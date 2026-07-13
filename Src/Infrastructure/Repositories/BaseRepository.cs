using Application.Interfaces.Repositories;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Paginations;

namespace Infrastructure.Repositories;

public abstract class BaseRepository<TId, TEntity>(AppDbContext db)
    : IBaseRepository<TId, TEntity>
    where TId : IEquatable<TId>, new()
    where TEntity : class, IEntity<TId>
{
    public virtual async Task<Result<PagedResult<TEntity>>> GetAllAsync(
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<TEntity>>.Success(
            await CustomContext()
                .PaginateAsync(paginationParameters, ct)
        );

    // Question: i think i shouldn't implement this method at all, IQueryable is Infrastructure concern
    public IQueryable<TEntity> GetAllAsQueryable()
        => CustomContext();

    public virtual async Task<Result<TEntity>> GetByIdAsync(
        TId id,
        CancellationToken ct)
    {
        var entity = await CustomContext().SingleOrDefaultAsync(e => e.Id.Equals(id), ct);
        if (entity is null)
            return Result<TEntity>.Failure(new Error($"{EntityName} with ID {id} not found", ErrorCode.NotFound),
                ResultCode.NotFound);
        return Result<TEntity>.Success(entity);
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

    // Question: do i need to check for duplicated IDs in a separate service?
    public virtual async Task<bool> ExistsAsync(
            TId id,
            CancellationToken ct)
        // => (await CustomContext().CountAsync(e => e.Id.Equals(id), ct)) switch
        // {
        //     0 => Result<bool>.Success(false),
        //     1 => Result<bool>.Success(true),
        //     _ => Result<bool>.Failure(new Error($"more than one {EntityName}s with id {id} found"))
        // };
        => await db.Set<TEntity>().AnyAsync(e => e.Id.Equals(id), ct);

    protected abstract IQueryable<TEntity> CustomContext();

    public virtual string EntityName => typeof(TEntity).Name;
}