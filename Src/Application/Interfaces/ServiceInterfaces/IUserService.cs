using Domain.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Enums;

namespace Application.Interfaces.ServiceInterfaces;

public interface IUserService
    : IBaseService<Guid, User>
{
    Task<User?> InsertAsync(User user, string password, CancellationToken ct);
    Task<bool> ExistsAsync(string phoneNumber, CancellationToken ct);
    Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken ct);
    Task<bool> PasswordChecks(User user, string password);
    Task<bool> RoleExistsAsync(UserRole role, CancellationToken ct);
    Task<IEnumerable<UserRole>> GetRolesAsync(User user, CancellationToken ct);
    Task<IEnumerable<UserRole>> GetRolesAsync(Guid userId, CancellationToken ct);
    Task<ICollection<Reservation>> GetReservationsAsync(Guid userId, CancellationToken ct);
}