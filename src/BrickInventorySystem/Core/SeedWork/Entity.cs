namespace BrickManager.BrickInventorySystem.Core.SeedWork;

public abstract class Entity<TIdentifier> : IEntity
{
    public TIdentifier Id { get; protected init; }
    
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; }

    protected virtual object Actual => this;

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (!(obj is Entity<TIdentifier> other))
            return false;

        if (Actual.GetType() != other.Actual.GetType())
            return false;

        if (Id!.Equals(default(TIdentifier)!))
            return false;

        if (other.Id!.Equals(default(TIdentifier)))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Id.Equals(other.Id);
    }

    public static bool operator !=(Entity<TIdentifier>? a, Entity<TIdentifier> b)
    {
        return !(a == b);
    }

    public static bool operator ==(Entity<TIdentifier>? a, Entity<TIdentifier>? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public void SetUpdatedDate()
        => UpdatedAt = DateTimeOffset.UtcNow;
}