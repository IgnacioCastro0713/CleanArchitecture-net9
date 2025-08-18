using Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel;

namespace Infrastructure.Database.Interceptors;

internal sealed class DispatchDomainEventsInterceptor(
    IDomainEventsDispatcher domainEventsDispatcher) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        int saved = await base.SavedChangesAsync(eventData, result, cancellationToken);

        await DispatchDomainEventsAsync(eventData.Context);

        return saved;
    }

    private async Task DispatchDomainEventsAsync(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var domainEvents = context.ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                List<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        await domainEventsDispatcher.DispatchAsync(domainEvents);
    }
}
