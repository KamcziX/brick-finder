using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BrickManager.BrickInventorySystem.Infrastructure.MySQL;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<EntityFrameworkDatabaseContext>
{
    private const string EnvironmentVariable = "ASPNETCORE_ENVIRONMENT";
    
    public EntityFrameworkDatabaseContext CreateDbContext(string[] args)
    {
        var environmentName = Environment.GetEnvironmentVariable(EnvironmentVariable);
        var configuration = BuildConfiguration(environmentName);
        
        var databaseOptions = configuration
            .GetSection(nameof(DatabaseSetupOptions))
            .Get<DatabaseSetupOptions>();
        
        var optionsBuilder = new DbContextOptionsBuilder<EntityFrameworkDatabaseContext>();
        var conStr = databaseOptions.ConnectionString;
        
        optionsBuilder.UseMySQL(conStr);
        optionsBuilder.EnableSensitiveDataLogging(true);

        var x = new ConfigurationBuilder().SetBasePath(Path.Combine(Directory.GetCurrentDirectory()));
        
        return new EntityFrameworkDatabaseContext(optionsBuilder.Options);
    }
    
    private static IConfiguration BuildConfiguration(string? environmentName)
        => new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("appSettings.json", true)
            .AddJsonFile($"appSettings.{environmentName}.json", true)
            .Build();
}