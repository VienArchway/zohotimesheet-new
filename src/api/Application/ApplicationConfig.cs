using api.Application.Interfaces;

namespace api.Application
{
    public static class ApplicationConfig
    {
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISprintService, SprintService>();
            services.AddScoped<ILogWorkService, LogWorkService>();
            services.AddScoped<ITaskItemService, TaskItemService>();
            services.AddScoped<IZohoTokenService, ZohoTokenService>();
            // services.AddScoped<IZohoWebHookService, ZohoWebHookService>();
            // services.AddScoped<ISlackService, SlackService>();
            // services.AddScoped<IWebHookService, WebHookService>();
            // services.AddScoped<IAdlsService, AdlsService>();

            // services.AddHostedService<QueuedHostedService>();
            // services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            // services.AddSingleton<ZohoHostedService>();
            // services.AddSingleton<IHostedService, ZohoHostedService>(serviceProvider => serviceProvider.GetService<ZohoHostedService>());
        }
    }
}