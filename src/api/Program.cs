using api.Application;
using api.Infrastructure;
using api.Services;
using api.Services.Security;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSpaStaticFiles(config => { config.RootPath = "dist"; });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add custom scheme authentication
builder.Services.AddAuthentication(ZohoAuthenticationSchema.DefaultSchemeName)
    .AddScheme<ZohoAuthenticationSchema, ZohoAuthenticationHandler>(ZohoAuthenticationSchema.DefaultSchemeName, options => {});

// register dependency injections
ServiceConfig.ConfigureService(builder);
ApplicationConfig.ConfigureService(builder.Services);
InfrastructureConfig.ConfigureService(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.Urls.Add("http://0.0.0.0:5000");
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Set up build docker
    app.Urls.Add("http://0.0.0.0:80");
}

builder.Configuration
    .SetBasePath(app.Environment.ContentRootPath)
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{app.Environment.EnvironmentName}.json", true,true)
    .AddEnvironmentVariables();

app.UseSpaStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });

// Register middleware spa client
app.UseSpa(spa =>
{
    if (app.Environment.IsDevelopment()) spa.UseProxyToSpaDevelopmentServer("http://localhost:3000/");
});

app.Run();