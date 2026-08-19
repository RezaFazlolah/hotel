using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Interfaces.Services;

public interface ICurrentUserService
{
    Result<Guid> Id { get; }
    Result<IReadOnlyList<UserRole>> Roles { get; }
    Result<(Guid id, IReadOnlyList<UserRole> roles)> Info { get; }
    Task<Result<User>> GetCurrentUserAsync(CancellationToken ct);
    bool IsAuthenticated();
}