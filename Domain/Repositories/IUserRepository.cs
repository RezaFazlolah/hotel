using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.Repositories;

public interface IUserRepository
{
    // Dictionary<UserRoles, string> RolesToString { get; }
    Task<bool> UserExistsAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<bool> PasswordChecks(User user, string password);

    Task<IdentityResult> RegisterAsync(User user, string password, string userRole,
        CancellationToken cancellationToken);

    Task<string?> GenerateJwt(User user);
    Task<IEnumerable<string>> GetRolesAsync(User user, CancellationToken cancellationToken);
}