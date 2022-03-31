using api;
using api.Application;
using api.Infrastructure;
using api.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddSpaStaticFiles(config => { config.RootPath = "dist"; });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add custom scheme authentication
builder.Services.AddAuthentication(ZohoSetting.DefaultSchemeName)
    .AddScheme<ZohoSetting, ZohoAuthHandle>(ZohoSetting.DefaultSchemeName, options => {});

// register dependency injections
ServiceConfig.ConfigureService(builder);
ApplicationConfig.ConfigureService(builder.Services);
InfrastructureConfig.ConfigureService(builder.Services);

var app = builder.Build();

// Set up build docker
// app.Urls.Add("http://0.0.0.0:5000");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

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