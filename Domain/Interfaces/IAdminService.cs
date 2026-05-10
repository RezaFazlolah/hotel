using Domain.Models;

namespace Domain.Interfaces;

public interface IAdminService
    : IUserService
{
    Task<ICollection<Reservation>> GetReservationsAsync(Guid adminId, CancellationToken ct);
}