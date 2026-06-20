using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Interfaces;

public interface ICurrentUserService
{
    Result<Guid> Id { get; }
    Task<Result<IEnumerable<UserRole>>> GetRolesAsync(CancellationToken ct);
}