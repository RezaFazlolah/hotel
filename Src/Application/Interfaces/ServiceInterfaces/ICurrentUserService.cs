using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Interfaces.ServiceInterfaces;

public interface ICurrentUserService
{
    Guid Id { get; }
    Task<Result<IEnumerable<UserRole>>> GetRolesAsync(CancellationToken ct);
}