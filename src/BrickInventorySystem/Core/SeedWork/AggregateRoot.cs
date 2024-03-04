namespace BrickManager.BrickInventorySystem.Core.SeedWork;

public abstract class AggregateRoot<TIdentifier> : Entity<TIdentifier>, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    public IReadOnlyCollection<IDomainEvent> DomainEvents 
        => _domainEvents;

    public void ClearDomainEvents()
        => _domainEvents.Clear();

    protected void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);
}