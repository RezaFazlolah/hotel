using Domain.Models;
using Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public abstract class BaseService<TEntity, TKey>(AppDbContext context)
    : IBaseService<TEntity, TKey>
    where TEntity : class, IBaseModel<TKey>
    where TKey : IEquatable<TKey>
{
    public virtual async Task<ICollection<TEntity>> GetAllAsync(
        CancellationToken cancellationToken,
        string? filterOn = null, string? filterQuery = null,
        string? orderBy = null, bool isAscending = true,
        int pageNumber = 1, int pageSize = int.MaxValue)
    {
        var query = CustomContext();

        // filtering
        query = CustomFilter(query, filterOn, filterQuery);
        // sorting
        query = CustomSort(query, orderBy, isAscending);
        // pagination
        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        return await CustomContext().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken: cancellationToken);
    }

    public virtual async Task<TEntity?> InsertAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<TEntity?> DeleteAsync(TKey id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null)
            return null;

        context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    protected abstract IQueryable<TEntity> CustomContext();

    protected abstract IQueryable<TEntity> CustomFilter(IQueryable<TEntity> query, string? filterOn,
        string? filterQuery);

    protected abstract IQueryable<TEntity> CustomSort(IQueryable<TEntity> query, string? orderBy, bool isAscending);
}