using Domain.Interfaces;
using SharedKernel.Common;
using SharedKernel.Filters;
using SharedKernel.Paginations;

namespace Application.Interfaces.QueryServices;

public interface IBaseQueryService<TEntity, TDto>
    where TEntity : class, IEntity<Guid>
{
    Task<Result<TDto>> GetByIdAsync(
        Guid id,
        CancellationToken ct);

    Task<Result<PagedResult<TDto>>> GetAllAsync(
        PaginationParameters paginationParameters,
        CancellationToken ct);

    public string EntityName { get; }
}