using Domain.Models;

namespace Application.Interfaces.ServiceInterfaces;

public interface IAdminService
    : IUserService
{
    Task<ICollection<Reservation>> GetReservationsAsync(Guid adminId, CancellationToken ct);
}