using api.Infrastructure.Interfaces;
using api.Infrastructure.Clients;
using Microsoft.Rest;
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Api;

namespace api.Infrastructure
{
    public static class InfrastructureConfig
    {
        public static void ConfigureService(IServiceCollection services)
        {
            
            services.AddScoped<IProjectClient, ProjectClient>();
            services.AddScoped<ITeamClient, TeamClient>();
            services.AddScoped<IUserClient, UserClient>();
            services.AddScoped<ITaskItemClient, TaskItemClient>();
            services.AddScoped<IBackLogClient, BackLogClient>();
            services.AddScoped<ISprintClient, SprintClient>();
            services.AddScoped<ILogWorkClient, LogWorkClient>();
            services.AddScoped<IZohoTokenClient, ZohoTokenClient>();
            services.AddScoped<IWebHookClient, WebHookClient>();
            services.AddScoped<IAdlsClient, AdlsClient>();
            services.AddScoped<ISlackWebHookClient, SlackWebHookClient>();
        }
    }
}