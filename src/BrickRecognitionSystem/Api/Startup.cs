using BrickManager.BrickRecognitionSystem.Application.Extensions;
using BrickManager.BrickRecognitionSystem.Application.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrickManager.BrickRecognitionSystem.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient();
        serviceCollection.AddOptions();
        serviceCollection.AddProblemDetails();

        serviceCollection.AddApplicationServices();

        serviceCollection.Configure<ObjectDetectionOptions>(_configuration.GetSection(nameof(ObjectDetectionOptions)));

        serviceCollection.AddHealthChecks();
        serviceCollection.AddControllers();
    }

    public void Configure(IApplicationBuilder applicationBuilder,
        IWebHostEnvironment webHostEnvironment)
    {
        applicationBuilder.UseHsts();
        applicationBuilder.UseExceptionHandler();
        applicationBuilder.UseStatusCodePages();
        applicationBuilder.UseRouting();
        
        applicationBuilder.UseEndpoints(
            endpointRouteBuilder => {                 
                endpointRouteBuilder.MapHealthChecks("/healthcheck")
                    .WithMetadata(new AllowAnonymousAttribute());});
    }
}