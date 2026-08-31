using Application.Interfaces.Repositories;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Repositories;

public abstract class RepositoryBase<TId, TEntity>(
    AppDbContext db,
    IDistributedCache cache)
    : IRepositoryBase<TId, TEntity>
    where TId : IEquatable<TId>
    where TEntity : class, IEntity<TId>
{
    public virtual async Task<Result<IReadOnlyList<TEntity>>> GetAllAsync(CancellationToken ct)
        => Result<IReadOnlyList<TEntity>>.Success(
            await db.Set<TEntity>().ToListAsync(ct));

    public virtual async Task<Result<TEntity>> GetByIdAsync(
        TId id,
        CancellationToken ct)
    {
        var entity = await db.Set<TEntity>()
            .FindAsync([id], ct);

        return entity is null
            ? Result<TEntity>.Failure(new Error($"{EntityName} with ID {id} not found", ErrorCode.NotFound),
                ResultCode.NotFound)
            : Result<TEntity>.Success(entity);
    }

    public virtual async Task<Result<TEntity>> AddAsync(
        TEntity entity,
        CancellationToken ct)
    {
        await db.Set<TEntity>().AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);

        // var options = new DistributedCacheEntryOptions()
        // await cache.SetAsync(entity.Id, JsonSerializer.SerializeToUtf8Bytes(entity), );

        return Result<TEntity>.Success(entity, ResultCode.Created);
    }

    public virtual async Task<Result<TEntity>> UpdateAsync(
        TEntity entity,
        CancellationToken ct)
    {
        var entityExists = await ExistsAsync(entity.Id, ct);
        if (!entityExists)
            return Result<TEntity>.Failure(new Error($"update {EntityName} {entity.Id} failed. {EntityName} not found.", ErrorCode
                .NotFound), ResultCode.NotFound);

        db.Set<TEntity>().Update(entity);
        await db.SaveChangesAsync(ct);

        return Result<TEntity>.Success(entity, ResultCode.Updated);
    }

    public virtual async Task<Result<TEntity>> DeleteAsync(
        TId id,
        CancellationToken ct)
    {
        var result = await GetByIdAsync(id, ct);
        if (!result.Succeeded)
            return result;
        var entity = result.Value;

        db.Set<TEntity>().Remove(entity);
        await db.SaveChangesAsync(ct);

        return Result<TEntity>.Success(entity, ResultCode.Deleted);
    }

    // Question: do I need to check for duplicated IDs in a separate service?
    public virtual async Task<bool> ExistsAsync(
        TId id,
        CancellationToken ct)
    {
        return await db.Set<TEntity>()
            .FindAsync([id], ct) != null;
    }

    public virtual string EntityName => typeof(TEntity).Name;
}