using BrickManager.BrickRecognitionSystem.Api;
using BrickManager.BrickRecognitionSystem.Api.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Starting Brick Recognition System");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
    .AddJsonFile($"appSettings.{builder.Environment.EnvironmentName}.json", false, true)
    .Build();
    
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
builder.Host.UseContentRoot(Directory.GetCurrentDirectory());

var app = builder.Build();

app.MapIdentificationEndpoints();

startup.Configure(app, app.Environment);
app.Run();