using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.Repositories;

public interface IRepositoryBase<in TId, TEntity>
{
    Task<Result<IReadOnlyList<TEntity>>> GetAllAsync(CancellationToken ct);

    Task<Result<TEntity>> GetByIdAsync(
        TId id,
        CancellationToken ct);

    Task<Result<TEntity>> AddAsync(
        TEntity entity,
        CancellationToken ct);

    Task<Result<TEntity>> UpdateAsync(
        TEntity entity,
        CancellationToken ct);

    Task<Result<TEntity>> UpdateWithReloadAsync(
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