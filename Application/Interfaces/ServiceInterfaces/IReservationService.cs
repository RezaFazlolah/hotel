using Domain.Models;

namespace Application.Interfaces.ServiceInterfaces;

public interface IReservationService
    : IBaseService<Guid, Reservation>
{
    // check if there is any reservation
    Task<bool> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        CancellationToken ct);

    // check if there is any reservation, but guestId is ignored, its mainly used for updating reservation
    Task<bool> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        Guid guestId, CancellationToken ct);

    Task<decimal> CalculateTotalPriceAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        CancellationToken ct);

    Task<Reservation?> CancelAsync(Guid reservationId, CancellationToken ct);
}