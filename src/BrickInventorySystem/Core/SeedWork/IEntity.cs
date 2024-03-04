namespace BrickManager.BrickInventorySystem.Core.SeedWork;

public interface IEntity
{
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; set; }

    void SetUpdatedDate();
}