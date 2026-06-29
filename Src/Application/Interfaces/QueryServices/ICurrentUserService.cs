using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Interfaces.QueryServices;

public interface ICurrentUserService
{
    Result<Guid> Id { get; }
    Result<IEnumerable<UserRole>> Roles { get; }
    Task<Result<User>> GetUserAsync(CancellationToken ct);
    Task<Result<(Guid id, User user, IEnumerable<UserRole> roles)>> GetUserInfoAsync(CancellationToken ct);
}