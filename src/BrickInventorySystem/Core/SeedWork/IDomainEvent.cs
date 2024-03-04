using MediatR;

namespace BrickManager.BrickInventorySystem.Core.SeedWork;

public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTimeOffset OccuredAt { get; }
}