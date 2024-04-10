using BrickManager.BrickRecognitionSystem.Api;
using BrickManager.BrickRecognitionSystem.Api.Endpoints;
using BrickManager.BrickRecognitionSystem.Application.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Starting Brick Recognition System");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerDocument(settings =>
    settings.PostProcess = document =>
    {
        document.Info.Title = "Brick Recognition System API"; 
    });

builder.Services.AddEndpointsApiExplorer();

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
    .AddJsonFile($"appSettings.{builder.Environment.EnvironmentName}.json", false, true)
    .Build();

var apiOptions = new ApiOptions();
configurationBuilder.GetSection(nameof(ApiOptions)).Bind(apiOptions);
    
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
builder.Host.UseContentRoot(Directory.GetCurrentDirectory());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

if (builder.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls(apiOptions.BaseUrls);
}

app.MapIdentificationEndpoints();

startup.Configure(app, app.Environment);
app.Run();