using SharedKernel.Common;

namespace Application.Interfaces.ServiceInterfaces;

public interface IBaseService<in TId, TEntity>
{
    Task<Result<ICollection<TEntity>>> GetAllAsync(CancellationToken cancellationToken, string? filterOn = null,
        string? filterQuery = null,
        string? orderBy = null, bool isAscending = true,
        int pageNumber = 1, int pageSize = int.MaxValue);

    Task<Result<TEntity>> GetByIdAsync(TId id, CancellationToken cancellationToken);
    Task<Result<TEntity>> InsertAsync(TEntity entity, CancellationToken cancellationToken);
    Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task<Result<TEntity>> DeleteAsync(TId id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken);
    string EntityName { get; }
}