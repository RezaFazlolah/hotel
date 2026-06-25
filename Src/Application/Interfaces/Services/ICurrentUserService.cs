using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Interfaces.Services;

public interface ICurrentUserService
{
    Result<Guid> UserId { get; }
    Result<User> User { get; }
    Result<IEnumerable<UserRole>> Roles { get; }
}