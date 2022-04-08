using api.Infrastructure.Interfaces;
using api.Infrastructure.Clients;

namespace api.Infrastructure
{
    public static class InfrastructureConfig
    {
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddScoped<IProjectClient, ProjectClient>();
            services.AddScoped<ITeamClient, TeamClient>();
            services.AddScoped<IZohoTokenClient, ZohoTokenClient>();
            services.AddScoped<ITaskItemClient, TaskItemClient>();
            services.AddScoped<IBackLogClient, BackLogClient>();
            services.AddScoped<ISprintClient, SprintClient>();
            services.AddScoped<IUserClient, UserClient>();
            services.AddScoped<ILogWorkClient, LogWorkClient>();
            services.AddScoped<IWebHookClient, WebHookClient>();
        }
    }
}