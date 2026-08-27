using SharedKernel.Common;

namespace Application.Interfaces.QueryServices;

public interface IQueryServiceBase<TDto>
{
    Task<Result<TDto>> GetByIdAsync(
        Guid id,
        CancellationToken ct);

    public string EntityName { get; }
}