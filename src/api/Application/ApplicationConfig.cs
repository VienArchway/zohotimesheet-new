using api.Application.Interfaces;

namespace api.Application
{
    public static class ApplicationConfig
    {
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
        }
    }
}