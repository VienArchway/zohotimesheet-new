using System.Net.Http.Headers;

namespace api.Infrastructure
{
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
    }
}