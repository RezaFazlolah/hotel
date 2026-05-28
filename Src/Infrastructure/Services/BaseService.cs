using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Extensions;
using SharedKernel.Filtering;
using SharedKernel.Paging;
using SharedKernel.Sorting;

namespace Infrastructure.Services;

public abstract class BaseService<TId, TEntity>(AppDbContext context)
    : IBaseService<TId, TEntity>
    where TId : IEquatable<TId>, new()
    where TEntity : class, IBaseModel<TId>
{
    public virtual async Task<Result<PagedResult<TEntity>>> GetAllAsync(PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var query = CustomContext();
        // query = Filter(query, filterParameters);
        // query = Sort(query, sortParameters);
        return Result<PagedResult<TEntity>>.Success(await query.ToPagedResultAsync(paginationParameters, ct));
    }

    public virtual async Task<Result<TEntity>> GetByIdAsync(TId id, CancellationToken ct)
    {
        try
        {
            var result = await CustomContext().SingleOrDefaultAsync(e => e.Id.Equals(id), ct);
            if (result is null)
                return Result<TEntity>.Failure(new Error($"{EntityName} with id {id} not found", ErrorCode.NotFound),
                    ResultCode.NotFound);
            return Result<TEntity>.Success(result);
        }
        catch
        {
            return Result<TEntity>.Failure(new Error($"more than one {EntityName}s with id {id} found"));
        }
    }

    public virtual async Task<Result<TEntity>> InsertAsync(TEntity entity, CancellationToken ct)
    {
        await context.Set<TEntity>().AddAsync(entity, ct);
        await context.SaveChangesAsync(ct);
        return Result<TEntity>.Success(entity, ResultCode.Created);
    }

    public virtual async Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken ct)
    {
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync(ct);
        return Result<TEntity>.Success(entity, ResultCode.Updated);
    }

    public virtual async Task<Result<TEntity>> DeleteAsync(TId id, CancellationToken ct)
    {
        var result = await GetByIdAsync(id, ct);
        if (!result.Succeeded)
            return result;

        var entity = result.Value;
        context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync(ct);
        return Result<TEntity>.Success(entity, ResultCode.Deleted);
    }

    public virtual async Task<bool> ExistsAsync(TId id, CancellationToken ct)
        // => (await CustomContext().CountAsync(e => e.Id.Equals(id), ct)) switch
        // {
        //     0 => Result<bool>.Success(false),
        //     1 => Result<bool>.Success(true),
        //     _ => Result<bool>.Failure(new Error($"more than one {EntityName}s with id {id} found"))
        // };
        => await context.Set<TEntity>().AnyAsync(e => e.Id.Equals(id), ct);
    // do i need to check for duplicated IDs in a separate service?

    protected abstract IQueryable<TEntity> CustomContext();

    // protected virtual IQueryable<TEntity> Filter(IQueryable<TEntity> query, BaseFilterParameters filterParameters)
    // {
    //     throw new NotImplementedException();
    // }

    // protected virtual IQueryable<TEntity> Sort(IQueryable<TEntity> query, BaseSortParameters sortParameters)
    // {
    //     throw new NotImplementedException();
    // }

    public virtual string EntityName => typeof(TEntity).Name;
}