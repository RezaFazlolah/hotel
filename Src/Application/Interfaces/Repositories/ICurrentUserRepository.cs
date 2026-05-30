using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Interfaces.Repositories;

public interface ICurrentUserRepository
{
    Guid Id { get; }
    Task<Result<IEnumerable<UserRole>>> GetRolesAsync(CancellationToken ct);
}