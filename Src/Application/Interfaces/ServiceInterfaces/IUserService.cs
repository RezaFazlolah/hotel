using Domain.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Enums;

namespace Application.Interfaces.ServiceInterfaces;

public interface IUserService
    : IBaseService<Guid, User>
{
    Task<bool> ExistsAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<bool> PasswordChecks(User user, string password);

    Task<IdentityResult> RegisterAsync(User user, string password, UserRole role,
        CancellationToken cancellationToken);

    Task<IEnumerable<UserRole>> GetRolesAsync(User user, CancellationToken cancellationToken);
    Task<IEnumerable<UserRole>> GetRolesAsync(Guid userId, CancellationToken cancellationToken);
}