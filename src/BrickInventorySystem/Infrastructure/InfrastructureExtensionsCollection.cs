using BrickManager.BrickInventorySystem.Core;
using BrickManager.BrickInventorySystem.Infrastructure.MySQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BrickManager.BrickInventorySystem.Infrastructure;

public static class InfrastructureExtensionsCollection
{
    public static void AddRepositories(this IServiceCollection serviceCollection)
    {
    }
    
    public static IServiceCollection AddUnitOfWork(this IServiceCollection serviceCollection)
        => serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
    
    public static IServiceCollection AddDataComponents(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        Action<DatabaseSetupOptions> setupAction)
    {
        serviceCollection.AddDatabaseProvider(configuration, setupAction);

        return serviceCollection;
    }
    
    private static void AddDatabaseProvider(this IServiceCollection serviceCollection,
        IConfiguration configuration,
        Action<DatabaseSetupOptions> setupAction)
    {
        serviceCollection.Configure(setupAction);
        serviceCollection.AddMySqlServer();
    }
    
    private static void AddMySqlServer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddServiceInitializer<DatabaseMigrator>();
        serviceCollection.AddDbContext<EntityFrameworkDatabaseContext>(
            (serviceProvider, optionsBuilder) =>
            {
                var connectionString = serviceProvider
                    .GetService<IOptionsSnapshot<DatabaseSetupOptions>>()!
                    .Value
                    .ConnectionString;
                    
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
    }
    
    private static IServiceCollection AddServiceInitializer<THostedService>(
        this IServiceCollection serviceCollection)
        where THostedService : class, IHostedService
    {
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<IHostedService, THostedService>());
        return serviceCollection;
    }
}