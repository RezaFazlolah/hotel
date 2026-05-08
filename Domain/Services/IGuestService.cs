using Domain.Models;

namespace Domain.Services;

public interface IGuestService
    : IUserService
{
    Task<ICollection<Reservation>> GetReservationsAsync(Guid guestId, CancellationToken ct);
}