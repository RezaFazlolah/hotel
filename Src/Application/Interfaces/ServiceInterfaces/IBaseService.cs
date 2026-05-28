using SharedKernel.Common;
using SharedKernel.Filtering;
using SharedKernel.Paging;
using SharedKernel.Sorting;

namespace Application.Interfaces.ServiceInterfaces;

public interface IBaseService<in TId, TEntity>
{
    Task<Result<PagedResult<TEntity>>> GetAllAsync(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<Result<TEntity>> GetByIdAsync(TId id, CancellationToken cancellationToken);
    Task<Result<TEntity>> InsertAsync(TEntity entity, CancellationToken cancellationToken);
    Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task<Result<TEntity>> DeleteAsync(TId id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken);
    string EntityName { get; }
}