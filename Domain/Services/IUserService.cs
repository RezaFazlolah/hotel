using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.Services;

public interface IUserService
{
    Task<bool> UserExistsAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<bool> PasswordChecks(User user, string password);

    Task<IdentityResult> RegisterAsync(User user, string password, string role,
        CancellationToken cancellationToken);

    Task<string?> GenerateJwt(User user);
    Task<IEnumerable<string>> GetRolesAsync(User user, CancellationToken cancellationToken);
}