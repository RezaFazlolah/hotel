using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Interfaces.QueryServices;

public interface ICurrentUserService
{
    Result<Guid> Id { get; }
    Result<IReadOnlyList<UserRole>> Roles { get; }
    Task<Result<User>> GetUserAsync(CancellationToken ct);
    Task<Result<(Guid id, User user, IReadOnlyList<UserRole> roles)>> GetUserInfoAsync(CancellationToken ct);
}
