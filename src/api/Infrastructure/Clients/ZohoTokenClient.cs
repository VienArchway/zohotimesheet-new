using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using api.Models;
using api.Infrastructure.Interfaces;
using System.Net.Http.Headers;
using System.Security.Authentication;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace api.Infrastructure.Clients
{
    public class ZohoTokenClient: ZohoServiceClient, IZohoTokenClient
    {
        private ITeamClient teamClient;

        public ZohoTokenClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider, ITeamClient teamClient)
            : base(client, configuration, svcProvider)
        {
            this.teamClient = teamClient;
        }

        public async Task<Token> GetAccessTokenAsync(string code)
        {
            var tokenHost = configuration.GetValue<string>("Zoho:TokenHost");
            var clientId = configuration.GetValue<string>("Zoho:ClientId");
            var clientSecret = configuration.GetValue<string>("Zoho:ClientSecret");
            var redirectUri = configuration.GetValue<string>("Zoho:Redirect_uri");

            using var clientToken = new HttpClient();
            var parameter = new List<KeyValuePair<string, string>>
            {
                new("code", code),
                new("client_id", clientId),
                new("client_secret", clientSecret),
                new("redirect_uri", redirectUri),
                new("grant_type", "authorization_code"),
                new("prompt", "consent")
            };
            var content = new FormUrlEncodedContent(parameter);
            clientToken.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            var response = await clientToken.PostAsync($"{tokenHost}", content);
            var resContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var token = JsonConvert.DeserializeObject<Token>(resContent);
            if (token == null) return token ?? throw new InvalidOperationException();
            
            await FetchSecretRefreshToken(token, null).ConfigureAwait(false);

            return token ?? throw new InvalidOperationException();
        }

        public async Task<Token> GetAccessTokenFromRefreshTokenAsync(string firstName)
        {
            var refreshToken = await FetchSecretRefreshToken(null, firstName);
            if (refreshToken == null) throw new OperationCanceledException();
            
            var tokenHost = configuration.GetValue<string>("Zoho:TokenHost");
            var clientId = configuration.GetValue<string>("Zoho:ClientId");
            var clientSecret = configuration.GetValue<string>("Zoho:ClientSecret");

            using var clientToken = new HttpClient();
            var parameter = new List<KeyValuePair<string, string>>
            {
                new("refresh_token", refreshToken),
                new("client_id", clientId),
                new("client_secret", clientSecret),
                new("grant_type", "refresh_token")
            };
            var content = new FormUrlEncodedContent(parameter);
            clientToken.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            var response = await clientToken.PostAsync($"{tokenHost}", content);
            var resContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var token = JsonConvert.DeserializeObject<Token>(resContent);

            if (token?.AccessToken == null) throw new AuthenticationException("Access token null");
            return token ?? throw new InvalidOperationException();
        }

        public async Task RevokeRefreshTokenAsync()
        {
            var refreshToken = await FetchSecretRefreshToken(null, null);
            if (refreshToken == null) throw new OperationCanceledException();
            
            using var clientToken = new HttpClient();
            var parameter = new List<KeyValuePair<string, string>>
            {
                new("token", refreshToken)
            };
            var content = new FormUrlEncodedContent(parameter);
            clientToken.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            var response = await clientToken.PostAsync($"{configuration["Zoho:TokenHost"]}/revoke", content);
 
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

            using var clientToken = new HttpClient();
            var parameter = new List<KeyValuePair<string, string>>
            {
                new("refresh_token", refreshToken),
                new("client_id", clientId),
                new("client_secret", clientSecret),
                new("grant_type", "refresh_token")
            };
            var content = new FormUrlEncodedContent(parameter);
            clientToken.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            var response = await clientToken.PostAsync($"{tokenHost}", content);
            var resContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var token = JsonConvert.DeserializeObject<Token>(resContent);

            return token ?? throw new InvalidOperationException();
        }
        
        private async Task<string> FetchSecretRefreshToken(Token? token, string? displayName)
        {
            var firstName = displayName ?? string.Empty;
            if (token is not null)
            {
                var teamSetting = await teamClient.GetTeamSettingAsync("signout", token.AccessToken).ConfigureAwait(false);
                firstName = teamSetting?["firstName"]?.ToString().Replace(" ", "");
            }

            var secretClient = new SecretClient(
                new Uri("https://zohotoken.vault.azure.net/"),
                new DefaultAzureCredential());
            Response<KeyVaultSecret>? refreshToken = null;

            try
            {
                refreshToken = await secretClient
                    .GetSecretAsync($"refreshtoken-{firstName}").ConfigureAwait(false);
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e);
                if (e.Status == 404)
                {
                    refreshToken = await secretClient
                        .SetSecretAsync(
                            $"refreshtoken-{firstName}",
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
                            $"refreshtoken-{firstName}",
                            refreshToken.Value.Properties.Version
                        ).ConfigureAwait(false);
                    Console.WriteLine("Get current secret success");
                    
                    currentSecretAsync.Value.Properties.Enabled = false;
                    await secretClient.UpdateSecretPropertiesAsync(currentSecretAsync.Value.Properties).ConfigureAwait(false);
                    Console.WriteLine("Old secret has been disabled");
                    
                    refreshToken = await secretClient.SetSecretAsync(
                        $"refreshtoken-{firstName}", token?.RefreshToken).ConfigureAwait(false);
                    Console.WriteLine("New secret has updated");
                }
            }

            return refreshToken.Value.Value;
        }
    }
}