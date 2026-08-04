using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.Repositories;

public interface IBaseRepository<in TId, TEntity, TFilterParameters, TSortParameters>
{
    Task<Result<PagedResult<TEntity>>> GetAllAsync(
        TFilterParameters? filterParameters,
        TSortParameters sortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<TEntity>> GetByIdAsync(
        TId id,
        CancellationToken ct);

    Task<Result<TEntity>> InsertAsync(
        TEntity entity,
        CancellationToken ct);

    Task<Result<TEntity>> UpdateAsync(
        TEntity entity,
        CancellationToken ct);

    Task<Result<TEntity>> DeleteAsync(
        TId id,
        CancellationToken ct);

    Task<bool> ExistsAsync(
        TId id,
        CancellationToken ct);

    string EntityName { get; }
}