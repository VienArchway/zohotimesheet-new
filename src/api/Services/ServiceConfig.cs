using api.Services.HttpClient;
using Microsoft.Rest;
using Microsoft.PowerBI.Api;
using Microsoft.Identity.Client;

namespace api.Services
{
    public static class ServiceConfig
    {
        public static void ConfigureService(WebApplicationBuilder? builder)
        {
            builder?.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder?.Services.AddScoped(_ =>
            {
                var accessToken = AccessTokenForPowerBI(builder.Configuration);
                var token = new TokenCredentials(accessToken);
                var powerBIClient = new PowerBIClient(token);
                return powerBIClient;
            });

            HttpClientConfig.ConfigureService(builder);
        }

        private static string AccessTokenForPowerBI(IConfiguration configuration)
        {
            var tenantId = configuration.GetValue<string>("PowerBI:TenantId");
            var clientId = configuration.GetValue<string>("PowerBI:ClientId");
            var clientSecret = configuration.GetValue<string>("PowerBI:ClientSecret");
            var authorityUri = configuration.GetValue<string>("PowerBI:AuthorityUri");
            var scope = configuration.GetValue<string>("PowerBI:Scope");
            var tenantSpecificUrl = authorityUri.Replace("organizations", tenantId);
            var clientApp = ConfidentialClientApplicationBuilder
                                .Create(clientId)
                                .WithClientSecret(clientSecret)
                                .WithAuthority(tenantSpecificUrl)
                                .Build();
            var authenticationResult = clientApp.AcquireTokenForClient(new string[] { scope }).ExecuteAsync().Result;
            return authenticationResult.AccessToken;
        }
    }
}