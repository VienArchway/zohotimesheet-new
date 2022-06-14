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
                // new("prompt", "consent") // just enable when need to get new refresh token always
            };
            var content = new FormUrlEncodedContent(parameter);
            clientToken.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            var response = await clientToken.PostAsync($"{tokenHost}", content);
            var resContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var token = JsonConvert.DeserializeObject<Token>(resContent);

            if (token == null) return token ?? throw new InvalidOperationException();
            
            var secretClient = new SecretClient(
                new Uri("https://zohotoken.vault.azure.net/"),
                new DefaultAzureCredential());
            var refreshToken = await secretClient.GetSecretAsync(
                $"refreshtoken-{configuration["userName"]}", null, CancellationToken.None);
            if (string.IsNullOrEmpty(refreshToken.Value.Value))
            {
                await secretClient.SetSecretAsync(
                    $"refreshtoken-{configuration["userName"]}", token.RefreshToken, CancellationToken.None);
            }

            return token ?? throw new InvalidOperationException();
        }

        public async Task<Token> GetAccessTokenFromRefreshTokenAsync()
        {
            var secretClient = new SecretClient(
                new Uri("https://zohotoken.vault.azure.net/"),
                new DefaultAzureCredential());
            var refreshToken = await secretClient.GetSecretAsync(
                $"refreshtoken-{configuration["userName"]}", null, CancellationToken.None);
            if (refreshToken.Value.Value == null) throw new OperationCanceledException();
            var tokenHost = configuration.GetValue<string>("Zoho:TokenHost");
            var clientId = configuration.GetValue<string>("Zoho:ClientId");
            var clientSecret = configuration.GetValue<string>("Zoho:ClientSecret");

            using var clientToken = new HttpClient();
            var parameter = new List<KeyValuePair<string, string>>
            {
                new("refresh_token", refreshToken.Value.Value),
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

            if (token?.AccessToken == null) throw new AuthenticationException("Wrong refresh token");
            return token ?? throw new InvalidOperationException();
        }

        public async Task RevokeRefreshTokenAsync()
        {
            var secretClient = new SecretClient(
                new Uri("https://zohotoken.vault.azure.net/"),
                new DefaultAzureCredential());
            var refreshToken = await secretClient.GetSecretAsync($"refreshtoken-{configuration["userName"]}", null, CancellationToken.None);
            if (refreshToken.Value.Value == null) throw new OperationCanceledException();
            var tokenHost = configuration.GetValue<string>("Zoho:TokenHost");

            using var clientToken = new HttpClient();
            var parameter = new List<KeyValuePair<string, string>>
            {
                new("token", refreshToken.Value.Value)
            };
            var content = new FormUrlEncodedContent(parameter);
            clientToken.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            var response = await clientToken.PostAsync($"{tokenHost}/revoke", content);
 
            var resContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var srcJObj = JsonConvert.DeserializeObject<JObject>(resContent);
            var status = srcJObj?.GetValue("status")?.ToObject<string>();

            if (status is not "success")
            {
                throw new InvalidOperationException(resContent);
            }
        }
    }
}