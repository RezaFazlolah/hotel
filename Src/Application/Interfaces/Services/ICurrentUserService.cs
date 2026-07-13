using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Interfaces.Services;

public interface ICurrentUserService
{
    Result<Guid> Id { get; }
    Result<IReadOnlyList<UserRole>> Roles { get; }
    Task<Result<User>> GetCurrentUserAsync(CancellationToken ct);
    Task<Result<(Guid id, User user, IReadOnlyList<UserRole> roles)>> GetCurrentUserInfoAsync(CancellationToken ct);
}