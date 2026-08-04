using Domain.Interfaces;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.QueryServices;

public interface IBaseQueryService<TEntity, TDto, TFilterParameters, TSortParameters>
    where TEntity : class, IEntity<Guid>
{
    Task<Result<TDto>> GetByIdAsync(
        Guid id,
        CancellationToken ct);

    Task<Result<PagedResult<TDto>>> GetAllAsync(
        TFilterParameters? filterParameters,
        TSortParameters sortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    public string EntityName { get; }
}