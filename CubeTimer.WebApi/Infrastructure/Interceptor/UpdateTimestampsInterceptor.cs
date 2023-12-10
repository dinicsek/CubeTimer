using CubeTimer.WebApi.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CubeTimer.WebApi.Infrastructure.Interceptor;

public class UpdateTimestampsInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateTimestampedEntities(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateTimestampedEntities(DbContext context)
    {
        DateTime utcNow = DateTime.UtcNow;
        var entities = context.ChangeTracker.Entries<TimestampedEntity>().ToList();

        foreach (EntityEntry<TimestampedEntity> entry in entities)
        {
            if (entry.State == EntityState.Added)
            {
                SetCurrentPropertyValue(
                    entry, nameof(TimestampedEntity.CreatedAt), utcNow);
                SetCurrentPropertyValue(entry, nameof(TimestampedEntity.UpdatedAt), utcNow);
            }

            if (entry.State == EntityState.Modified)
            {
                SetCurrentPropertyValue(
                    entry, nameof(TimestampedEntity.UpdatedAt), utcNow);
            }
        }

        static void SetCurrentPropertyValue(
            EntityEntry entry,
            string propertyName,
            DateTime utcNow) =>
            entry.Property(propertyName).CurrentValue = utcNow;
    }
}