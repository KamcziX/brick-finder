namespace BrickManager.BrickInventorySystem.Core.SeedWork;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}