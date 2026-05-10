using Domain.Models;

namespace Application.Interfaces.ServiceInterfaces;

public interface IGuestService
    : IUserService
{
    Task<ICollection<Reservation>> GetReservationsAsync(Guid guestId, CancellationToken ct);
}