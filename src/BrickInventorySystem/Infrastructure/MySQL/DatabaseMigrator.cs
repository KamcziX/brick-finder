using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BrickManager.BrickInventorySystem.Infrastructure.MySQL;

public class DatabaseMigrator(IServiceScopeFactory serviceScopeFactory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var databaseContext = scope.ServiceProvider.GetService<EntityFrameworkDatabaseContext>();

        try
        {
            await databaseContext!.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR unable to apply database migrations");
            Console.WriteLine("ERROR occured: " + e);
            throw;
        }

        Console.WriteLine("Database migrations applied successfully.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}