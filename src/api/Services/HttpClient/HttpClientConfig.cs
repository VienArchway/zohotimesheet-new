using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;

namespace api.Services.HttpClient;

public static class HttpClientConfig {
    public static void ConfigureService(WebApplicationBuilder? builder) {
        var zohoSprintApiHost = builder.Configuration.GetValue<string>("Zoho:ApiHost");

        builder.Services.AddHttpClient("", (services, httpClient) =>
        {
            var httpContext = services.GetRequiredService<IHttpContextAccessor>();

            // Get accessToken from Request header to reduce number of access tokens.
            // If we generate many token at the same time, Zoho will return error "Given token is invalid".
            // If request does not include Access Token in header, we will generate new one for that request.
            var accessToken = httpContext.HttpContext?.Request?.Headers["Zoho-Verify-Token"];

            // var accessToken =  ZohoSecurityHelper.GetAccessToken(services, builder.Configuration);

            httpClient.BaseAddress = new Uri(zohoSprintApiHost);

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Archway");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken);
        });
    }
}