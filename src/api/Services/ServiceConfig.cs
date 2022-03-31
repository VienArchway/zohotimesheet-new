using api.Infrastructure.Interfaces;
using api.Infrastructure.Clients;
using api.Services.HttpClient;

namespace api.Services
{
    public static class ServiceConfig
    {
        public static void ConfigureService(WebApplicationBuilder? builder)
        {
            builder?.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            HttpClientConfig.ConfigureService(builder);
        }
    }
}