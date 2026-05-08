using Domain.Models;

namespace Domain.Services;

public interface IAdminService
    : IUserService
{
    Task<ICollection<Reservation>> GetReservationsAsync(Guid adminId, CancellationToken ct);
}