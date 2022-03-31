using System.Net.Http.Headers;

namespace api.Service.Security
{
    public record TeamClient(object Portals, string Status);

    public static class ZohoSecurityHelper
    {
        public static string GetAccessToken(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var userRefreshToken = httpContext?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "ZohoRefreshToken")?.Value;

            return GetAccessTokenFromRefreshToken(configuration, userRefreshToken);
        }

        private static string GetAccessTokenFromRefreshToken(IConfiguration configuration, string refreshToken)
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
                var response = clientToken.PostAsync($"{tokenHost}", content).GetAwaiter().GetResult();
                var resContent = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                // return JsonConvert.DeserializeObject<AccessTokenResponse>(resContent).AccessToken;
                return "";
            }
        }


        public static async Task<string> GetTea(string apiKey, string path, IConfiguration config)
        {
            using (var httpClient = new HttpClient())
            {
                if (httpClient.BaseAddress == null)
                {
                    httpClient.BaseAddress = new Uri(config["Zoho:BaseUrl"]);
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {apiKey}");
                }
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Archway");
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.GetAsync(httpClient.BaseAddress + path);
                var teamClient = await response.Content.ReadFromJsonAsync<TeamClient>();
                Console.WriteLine(teamClient.Portals);
                return teamClient.Status;
            }
        }
    }
}