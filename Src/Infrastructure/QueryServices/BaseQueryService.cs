using Application.Interfaces.QueryServices;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Infrastructure.QueryServices;

public abstract class BaseQueryService<TEntity, TDto>(
    AppDbContext db,
    IConfigurationProvider configurationProvider)
    : IBaseQueryService<TEntity, TDto>
    where TEntity : class, IEntity<Guid>
{
    public virtual async Task<Result<TDto>> GetByIdAsync(
        Guid id,
        CancellationToken ct)
    {
        var dto = await db.Set<TEntity>()
            .Where(e => e.Id == id)
            .ProjectTo<TDto>(configurationProvider)
            .SingleOrDefaultAsync(ct);

        return dto is null
            ? Result<TDto>.Failure(new Error($"{EntityName} {id} not found."))
            : Result<TDto>.Success(dto);
    }

    public virtual async Task<Result<PagedResult<TDto>>> GetAllAsync(
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var dtos = await db.Set<TEntity>()
            .ProjectTo<TDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct);

        return Result<PagedResult<TDto>>.Success(dtos);
    }
    
    public virtual string EntityName => typeof(TEntity).Name;
}