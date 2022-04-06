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
            services.AddScoped<ITokenClient, TokenClient>();
            services.AddScoped<ITaskItemClient, TaskItemClient>();
        }
    }
}