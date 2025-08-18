using Application.Abstractions.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel;

namespace Infrastructure.Database.Interceptors;

internal class AuditableEntityInterceptor(
    IUserContext userContext,
    TimeProvider timeProvider) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        IEnumerable<EntityEntry<AuditableEntity>> entries = context.ChangeTracker
            .Entries<AuditableEntity>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified ||
                            entry.HasChangedOwnedEntities());

        foreach (EntityEntry<AuditableEntity> entry in entries)
        {
            DateTimeOffset utcNow = timeProvider.GetUtcNow();

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = userContext.UserId.ToString();
                entry.Entity.Created = utcNow;
            }

            entry.Entity.LastModifiedBy = userContext.UserId.ToString();
            entry.Entity.LastModified = utcNow;
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}
