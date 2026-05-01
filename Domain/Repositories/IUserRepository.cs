using Domain.Enums;
using Domain.Models;

namespace Domain.Repositories;

public interface IUserRepository
{
    Dictionary<UserRoles, string> RolesToString { get; }
    Task<bool> UserExistsAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<bool> PasswordChecks(User user, string password);
    Task<bool> RegisterAsync(User user, string password, UserRoles userRole, CancellationToken cancellationToken);
    Task<string?> LoginAsync(User user, string password, CancellationToken cancellationToken);
    string? CreateJwt(User user, IEnumerable<UserRoles> userRole);
    Task<IEnumerable<UserRoles>> GetRolesAsync(User user, CancellationToken cancellationToken);
}