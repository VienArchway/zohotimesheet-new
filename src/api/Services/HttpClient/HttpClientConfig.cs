using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;

namespace api.Services.HttpClient;

public static class HttpClientConfig {
    public static void Setup(WebApplicationBuilder? builder) {
        var zohoSprintApiHost = builder.Configuration.GetValue<string>("Zoho:ApiHost");

        builder.Services.AddHttpClient("", (services, httpClient) =>
        {
            var accessToken =  ZohoSecurityHelper.GetAccessToken(services, builder.Configuration);

            httpClient.BaseAddress = new Uri(zohoSprintApiHost);

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Archway");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken);
        });
    }
}