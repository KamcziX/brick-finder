namespace BrickManager.BrickInventorySystem.Core.SeedWork;

public class DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccuredAt = DateTimeOffset.UtcNow;
    }
    
    public Guid EventId { get; }
    public DateTimeOffset OccuredAt { get; }
}