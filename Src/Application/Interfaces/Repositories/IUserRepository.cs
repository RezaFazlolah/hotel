using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Paging;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
    : IBaseRepository<Guid, User>
{
    Task<Result> InsertAsync(User user, string password, CancellationToken ct);
    Task<Result> DeleteAsync(User user, CancellationToken ct);
    Task<bool> ExistsAsync(string phoneNumber, CancellationToken ct);
    Task<Result<User>> GetByPhoneNumberAsync(string phoneNumber, CancellationToken ct);
    Task<bool> CheckPassword(User user, string password);
    Task<bool> RoleExistsAsync(UserRole role, CancellationToken ct);
    Task<Result<IEnumerable<UserRole>>> GetRolesAsync(User user, CancellationToken ct);
    Task<Result<IEnumerable<UserRole>>> GetRolesAsync(Guid userId, CancellationToken ct);

    Task<Result<PagedResult<Reservation>>> GetAllReservationsAsync(Guid userId,
        PaginationParameters paginationParameters,
        CancellationToken ct);
    
    Task<Result> AddRoleAsync(User user, UserRole role, CancellationToken ct);
    
}