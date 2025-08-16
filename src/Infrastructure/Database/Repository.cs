using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Database;

internal abstract class Repository<TEntity>(ApplicationDbContext context)
    where TEntity : Entity
{
    protected readonly ApplicationDbContext DbContext = context;

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>().FindAsync([id], cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }

    public void AddRange(params IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
    }

    public void UpdateRange(params IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().UpdateRange(entities);
    }

    public void Remove(TEntity entity)
    {
        DbContext.Set<Entity>().Remove(entity);
    }

    public void RemoveRange(params IEnumerable<TEntity> entities)
    {
        DbContext.Set<Entity>().RemoveRange(entities);
    }
}
