using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Interfaces.Services;

public interface ICurrentUserService
{
    Result<Guid> Id { get; }
    // Task<Result<IEnumerable<UserRole>>> GetRolesAsync(CancellationToken ct);
    Result<User> User { get; }
    Result<IEnumerable<UserRole>> Roles { get; }
}