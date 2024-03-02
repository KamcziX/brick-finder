using Microsoft.EntityFrameworkCore;

namespace BrickManager.BrickInventorySystem.Infrastructure.MySQL;

public class EntityFrameworkDatabaseContext(DbContextOptions<EntityFrameworkDatabaseContext> dbContextOptions)
    : DbContext(dbContextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}