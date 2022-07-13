using api.Services.HttpClient;
using Microsoft.Rest;
using Microsoft.PowerBI.Api;
using Microsoft.Identity.Client;

namespace api.Services
{
    public static class ServiceConfig
    {
        public static void ConfigureService(WebApplicationBuilder builder)
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

        private static String AccessTokenForPowerBI(IConfiguration configuration)
        {
            var tenantId = configuration.GetValue<String>("PowerBI:TenantId");
            var clientId = configuration.GetValue<String>("PowerBI:ClientId");
            var clientSecret = configuration.GetValue<String>("PowerBI:ClientSecret");
            var authorityUri = configuration.GetValue<String>("PowerBI:AuthorityUri");
            var scope = configuration.GetValue<String>("PowerBI:Scope");
            var tenantSpecificUrl = authorityUri.Replace("organizations", tenantId);
            var clientApp = ConfidentialClientApplicationBuilder
                                .Create(clientId)
                                .WithClientSecret(clientSecret)
                                .WithAuthority(tenantSpecificUrl)
                                .Build();
            var authenticationResult = clientApp.AcquireTokenForClient(new String[] { scope }).ExecuteAsync().Result;
            return authenticationResult.AccessToken;
        }
    }
}