using System.Net.Http.Headers;

namespace api.Services.HttpClient;

public static class HttpClientConfig {
    public static void ConfigureService(WebApplicationBuilder builder) {
        var zohoSprintApiHost = builder.Configuration.GetValue<String>("Zoho:ApiHost");

        builder.Services.AddHttpClient("", (services, httpClient) =>
        {
            var httpContext = services.GetRequiredService<IHttpContextAccessor>();
            var accessToken = httpContext.HttpContext?.Request.Cookies["accessToken"];

            httpClient.BaseAddress = new Uri(zohoSprintApiHost);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Archway");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken);
        });
    }
}