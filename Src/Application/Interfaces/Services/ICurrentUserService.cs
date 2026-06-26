using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Interfaces.Services;

public interface ICurrentUserService
{
    Result<Guid> Id { get; }
    Result<User> User { get; }
    Result<IEnumerable<UserRole>> Roles { get; }
    Result<(Guid id, User user, IEnumerable<UserRole> roles)> Info { get; }
}