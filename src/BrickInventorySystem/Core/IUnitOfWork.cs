namespace BrickManager.BrickInventorySystem.Core;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}