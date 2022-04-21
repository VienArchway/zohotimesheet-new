using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using api.Models;
using api.Infrastructure.Interfaces;
using System.Net.Http.Headers;

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

            using (var clientToken = new HttpClient())
            {
                var parameter = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("grant_type", "authorization_code")
                };
                var content = new FormUrlEncodedContent(parameter);
                clientToken.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                var response = await clientToken.PostAsync($"{tokenHost}", content);
                var resContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var token = JsonConvert.DeserializeObject<Token>(resContent);
                // return JsonConvert.DeserializeObject<AccessTokenResponse>(resContent).AccessToken;
                return token;
            }
        }

        public async Task<Token> GetAccessTokenFromRefreshTokenAsync(string refreshToken)
        {
            var tokenHost = configuration.GetValue<string>("Zoho:TokenHost");
            var clientId = configuration.GetValue<string>("Zoho:ClientId");
            var clientSecret = configuration.GetValue<string>("Zoho:ClientSecret");
            var redirectUri = configuration.GetValue<string>("Zoho:Redirect_uri");

            using (var clientToken = new HttpClient())
            {
                var parameter = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("refresh_token", refreshToken),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("grant_type", "refresh_token")
                };
                var content = new FormUrlEncodedContent(parameter);
                clientToken.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                var response = await clientToken.PostAsync($"{tokenHost}", content);
                var resContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var token = JsonConvert.DeserializeObject<Token>(resContent);

                return token;
            }
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {
            var tokenHost = configuration.GetValue<string>("Zoho:TokenHost");

            using (var clientToken = new HttpClient())
            {
                var parameter = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("token", token)
                };
                var content = new FormUrlEncodedContent(parameter);
                clientToken.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                clientToken.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                var response = await clientToken.PostAsync($"{tokenHost}/revoke", content);
                
                var resContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var srcJObj = JsonConvert.DeserializeObject<JObject>(resContent);
                var status = srcJObj.GetValue("status")?.ToObject<string>();

                if (status == null || !status.Equals("success"))
                {
                    throw new InvalidOperationException(resContent);
                }
            }
        }
    }
}