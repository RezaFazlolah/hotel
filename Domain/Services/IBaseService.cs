namespace Domain.Services;

public interface IBaseService<in TId, TEntity>
{
    Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken, string? filterOn = null,
        string? filterQuery = null,
        string? orderBy = null, bool isAscending = true,
        int pageNumber = 1, int pageSize = int.MaxValue);

    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken);
    Task<TEntity?> InsertAsync(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity?> DeleteAsync(TId id, CancellationToken cancellationToken);
}