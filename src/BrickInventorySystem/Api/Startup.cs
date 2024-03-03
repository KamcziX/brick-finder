using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrickManager.BrickInventorySystem.Api;

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
        
        serviceCollection.AddHealthChecks();
        serviceCollection.AddControllers();
    }

    public void Configure(IApplicationBuilder applicationBuilder,
        IWebHostEnvironment webHostEnvironment)
    {
        applicationBuilder.UseHsts();
        applicationBuilder.UseRouting();
        
        applicationBuilder.UseEndpoints(
            endpointRouteBuilder => {                 
                endpointRouteBuilder.MapHealthChecks("/healthcheck")
                    .WithMetadata(new AllowAnonymousAttribute());});
    }
}