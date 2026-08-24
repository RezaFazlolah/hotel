using Application.Interfaces.Repositories;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Repositories;

public abstract class BaseRepository<TId, TEntity>(AppDbContext db)
    : IBaseRepository<TId, TEntity>
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

    public virtual async Task<Result<TEntity>> InsertAsync(
        TEntity entity,
        CancellationToken ct)
    {
        await db.Set<TEntity>().AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return Result<TEntity>.Success(entity, ResultCode.Created);
    }

    public virtual async Task<Result<TEntity>> UpdateAsync(
        TEntity entity,
        CancellationToken ct)
    {
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
        => await db.Set<TEntity>()
            .AnyAsync(e => e.Id.Equals(id), ct);

    public virtual string EntityName => typeof(TEntity).Name;
}