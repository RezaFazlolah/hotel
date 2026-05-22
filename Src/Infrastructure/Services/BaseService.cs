using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Services;

public abstract class BaseService<TId, TEntity>(AppDbContext context)
    : IBaseService<TId, TEntity>
    where TId : IEquatable<TId>, new()
    where TEntity : class, IBaseModel<TId>
{
    public virtual async Task<Result<ICollection<TEntity>>> GetAllAsync(
        CancellationToken ct,
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

        return Result<ICollection<TEntity>>.Success(await query.ToListAsync(ct));
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

    protected abstract IQueryable<TEntity> CustomFilter(IQueryable<TEntity> query, string? filterOn,
        string? filterQuery);

    protected abstract IQueryable<TEntity> CustomSort(IQueryable<TEntity> query, string? orderBy, bool isAscending);
    
    public virtual string EntityName => typeof(TEntity).Name;
}