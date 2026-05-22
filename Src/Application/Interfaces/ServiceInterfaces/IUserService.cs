using Domain.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Interfaces.ServiceInterfaces;

public interface IUserService
    : IBaseService<Guid, User>
{
    Task<Result<User>> InsertAsync(User user, string password, CancellationToken ct);
    Task<bool> ExistsAsync(string phoneNumber, CancellationToken ct);
    Task<Result<User>> GetByPhoneNumberAsync(string phoneNumber, CancellationToken ct);
    Task<bool> PasswordChecks(User user, string password);
    Task<bool> RoleExistsAsync(UserRole role, CancellationToken ct);
    Task<Result<IEnumerable<UserRole>>> GetRolesAsync(User user, CancellationToken ct);
    Task<Result<IEnumerable<UserRole>>> GetRolesAsync(Guid userId, CancellationToken ct);
    Task<Result<ICollection<Reservation>>> GetReservationsAsync(Guid userId, CancellationToken ct);
}