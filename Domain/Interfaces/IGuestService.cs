using Domain.Models;

namespace Domain.Interfaces;

public interface IGuestService
    : IUserService
{
    Task<ICollection<Reservation>> GetReservationsAsync(Guid guestId, CancellationToken ct);
}