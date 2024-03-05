using BrickManager.BrickInventorySystem.Core;
using BrickManager.BrickInventorySystem.Core.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BrickManager.BrickInventorySystem.Infrastructure.MySQL;

public class UnitOfWork(EntityFrameworkDatabaseContext databaseContext, IServiceScopeFactory serviceScopeFactory) : IUnitOfWork
{
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        var aggregates = databaseContext
            .ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(x => x.Entity)
            .ToList();
        
        var entities = databaseContext
            .ChangeTracker
            .Entries<IEntity>()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity)
            .ToList();
        
        var domainEvents = aggregates
            .SelectMany(x => x.DomainEvents)
            .ToList();
        
        aggregates.ForEach(x => x.ClearDomainEvents());
        entities.ForEach(x=> x.SetUpdatedDate());
        
        var changesCount = await databaseContext.SaveChangesAsync(cancellationToken);
        
        var scopes = new List<IServiceScope>();
        
        // Get all new domain events awaiting to be published
        var publishTasks = domainEvents.Select(x =>
        {
            var scope = serviceScopeFactory.CreateScope();
            var notificationPublisher = scope.ServiceProvider.GetService<IPublisher>();

            scopes.Add(scope);

            return notificationPublisher!.Publish(x, cancellationToken);
        });
        
        // Publish all domain events
        await Task.WhenAll(publishTasks);
        
        scopes.ForEach(x => x.Dispose());

        return changesCount;
    }
}