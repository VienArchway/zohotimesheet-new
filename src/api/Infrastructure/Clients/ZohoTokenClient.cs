using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using api.Models;
using api.Infrastructure.Interfaces;
using System.Net.Http.Headers;
using System.Security.Authentication;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using JsonSerializer = System.Text.Json.JsonSerializer;
using StringWithQualityHeaderValue = System.Net.Http.Headers.StringWithQualityHeaderValue;

namespace api.Infrastructure.Clients
{
    public class ZohoTokenClient: ZohoServiceClient, IZohoTokenClient
    {
        private readonly ITeamClient teamClient;

        public ZohoTokenClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider, ITeamClient teamClient)
            : base(client, configuration, svcProvider)
        {
            this.teamClient = teamClient;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        }

        public async Task<Token> GetAccessTokenAsync(string code)
        {
            var parameter = new List<KeyValuePair<string, string>>
            {
                new("code", code),
                new("client_id", configuration["Zoho:ClientId"]),
                new("client_secret", configuration["Zoho:ClientSecret"]),
                new("redirect_uri", configuration["Zoho:Redirect_uri"]),
                new("grant_type", "authorization_code"),
                new("prompt", "consent")
            };
            var request = new HttpRequestMessage(HttpMethod.Post, configuration["Zoho:TokenHost"]);
            request.Content = new FormUrlEncodedContent(parameter);
            
            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            
            var resContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var token = await JsonSerializer.DeserializeAsync<Token>(resContent);
            
            if (token == null) throw new InvalidOperationException();
            await FetchSecretRefreshToken(token, null).ConfigureAwait(false);

            return token ?? throw new InvalidOperationException();
        }

        public async Task<Token> GetAccessTokenFromRefreshTokenAsync(string? firstName, string? zsUserId)
        {
            var refreshToken = await FetchSecretRefreshToken(null, firstName, zsUserId).ConfigureAwait(false);
            if (refreshToken == null) throw new OperationCanceledException();
            
            var parameter = new List<KeyValuePair<string, string>>
            {
                new("refresh_token", refreshToken),
                new("client_id", configuration["Zoho:ClientId"]),
                new("client_secret", configuration["Zoho:ClientSecret"]),
                new("grant_type", "refresh_token")
            };
            var request = new HttpRequestMessage(HttpMethod.Post, configuration["Zoho:TokenHost"]);
            request.Content = new FormUrlEncodedContent(parameter);
            
            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            
            var resContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var token = await JsonSerializer.DeserializeAsync<Token>(resContent);

            if (token?.AccessToken == null) throw new AuthenticationException("Access token null");
            return token ?? throw new InvalidOperationException();
        }

        public async Task RevokeRefreshTokenAsync()
        {
            var refreshToken = await FetchSecretRefreshToken();
            if (refreshToken == null) throw new OperationCanceledException();
            
            var parameter = new List<KeyValuePair<string, string>>
            {
                new("token", refreshToken)
            };
            var content = new FormUrlEncodedContent(parameter);
            var response = await client.PostAsync($"{configuration["Zoho:TokenHost"]}/revoke", content);
 
            var resContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var srcJObj = JsonConvert.DeserializeObject<JObject>(resContent);
            var status = srcJObj?.GetValue("status")?.ToObject<string>();

            if (status is not "success")
            {
                throw new InvalidOperationException(resContent);
            }
        }

        public async Task<Token> GetAdminAccessTokenAsync()
        {
            var tokenHost = configuration.GetValue<string>("Zoho:TokenHost");
            var clientId = configuration.GetValue<string>("Zoho:ClientId");
            var clientSecret = configuration.GetValue<string>("Zoho:ClientSecret");
            var refreshToken = configuration.GetValue<string>("Zoho:RefreshToken");

            var parameter = new List<KeyValuePair<string, string>>
            {
                new("refresh_token", refreshToken),
                new("client_id", clientId),
                new("client_secret", clientSecret),
                new("grant_type", "refresh_token")
            };
            var content = new FormUrlEncodedContent(parameter);

            using var response = await client.PostAsync($"{tokenHost}", content);
            var resContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var token = JsonConvert.DeserializeObject<Token>(resContent);

            return token ?? throw new InvalidOperationException();
        }
        
        private async Task<string> FetchSecretRefreshToken(Token? token = null, string? firstName = null, string? zsUserId = null)
        {
            if (token is not null)
            {
                var settingForName =  teamClient.GetTeamSettingAsync("signout", token?.AccessToken);
                var settingForZsUserId = teamClient.GetTeamSettingAsync(null, token?.AccessToken);
                await Task.WhenAll(settingForName, settingForZsUserId).ConfigureAwait(false);

                firstName = settingForName.Result?.FirstName?.Replace(" ", "");
                zsUserId = settingForZsUserId.Result?.ZsUserId;
            }
            var tokenName = $"{firstName}-{zsUserId}";

            var secretClient = new SecretClient(
                new Uri("https://zohotoken.vault.azure.net/"),
                new DefaultAzureCredential());
            Response<KeyVaultSecret>? refreshToken = null;

            try
            {
                refreshToken = await secretClient
                    .GetSecretAsync($"refreshtoken-{tokenName}").ConfigureAwait(false);
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e);
                if (e.Status == 404)
                {
                    refreshToken = await secretClient
                        .SetSecretAsync(
                            $"refreshtoken-{tokenName}",
                            token?.RefreshToken,
                            CancellationToken.None).ConfigureAwait(false);
                    Console.WriteLine("New secret has created");
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                var newRefreshToken = token?.RefreshToken?.Equals(refreshToken?.Value.Value);
                if (!string.IsNullOrEmpty(token?.RefreshToken) && newRefreshToken is false)
                {
                    var currentSecretAsync = await secretClient
                        .GetSecretAsync(
                            $"refreshtoken-{tokenName}",
                            refreshToken?.Value.Properties.Version
                        ).ConfigureAwait(false);
                    Console.WriteLine("Get current secret success");
                    
                    currentSecretAsync.Value.Properties.Enabled = false;
                    await secretClient.UpdateSecretPropertiesAsync(currentSecretAsync.Value.Properties).ConfigureAwait(false);
                    Console.WriteLine("Old secret has been disabled");
                    
                    refreshToken = await secretClient.SetSecretAsync(
                        $"refreshtoken-{tokenName}", token?.RefreshToken).ConfigureAwait(false);
                    Console.WriteLine("New secret has updated");
                }
            }

            return refreshToken.Value.Value;
        }
    }
}