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
        public ZohoTokenClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
            : base(client, configuration, svcProvider)
        {
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
            
            // get user
            await FetchSecretRefreshToken(token);

            return token ?? throw new InvalidOperationException();
        }

        public async Task<Token> GetAccessTokenFromRefreshTokenAsync()
        {
            var refreshToken = await FetchSecretRefreshToken(null);
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
            var refreshToken = await FetchSecretRefreshToken(null);
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
        
        private async Task<ZohoUser> GetZohoUserInfo(string accessToken)
        {
            client.DefaultRequestHeaders.Add("User-Agent", "Archway");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken);
            var resUser = await client.GetAsync(configuration["Zoho:UserHost"]);
            var userInfo = await resUser.Content.ReadAsStringAsync().ConfigureAwait(false);
            var user = JsonConvert.DeserializeObject<ZohoUser>(userInfo);

            return user ?? throw new InvalidOperationException();
        }
        
        private async Task<string> FetchSecretRefreshToken(Token? token)
        {
            if (token is not null)
            {
                var zohoUser = await GetZohoUserInfo(token.AccessToken);
                Environment.SetEnvironmentVariable("displayName", zohoUser.DisplayName);
            }
            
            var secretClient = new SecretClient(
                new Uri("https://zohotoken.vault.azure.net/"),
                new DefaultAzureCredential());
            Response<KeyVaultSecret> refreshToken;

            try
            {
                refreshToken = await secretClient.GetSecretAsync(
                    $"refreshtoken-Archway-Ha", null, CancellationToken.None);
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e);
                if (e.Status == 404)
                {
                    refreshToken = await secretClient.SetSecretAsync(
                        $"refreshtoken-{Environment.GetEnvironmentVariable("displayName")}", token.RefreshToken, CancellationToken.None);
                }
                else
                {
                    throw;
                }
            }

            return refreshToken.Value.Value;
        }
    }
}