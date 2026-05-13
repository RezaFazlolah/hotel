using Domain.Models;

namespace Application.Interfaces.ServiceInterfaces;

public interface IReservationService
    : IBaseService<Guid, Reservation>
{
    Task<decimal> CalculateTotalPriceAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        CancellationToken ct);

    Task<Reservation?> CancelAsync(Guid reservationId, CancellationToken ct);
}