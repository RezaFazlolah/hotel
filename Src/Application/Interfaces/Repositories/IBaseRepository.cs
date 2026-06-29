using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Interfaces.Repositories;

public interface IBaseRepository<in TId, TEntity>
{
    Task<Result<PagedResult<TEntity>>> GetAllAsync(
        PaginationParameters paginationParameters,
        CancellationToken ct);

    IQueryable<TEntity> GetAllAsQueryable();
    
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