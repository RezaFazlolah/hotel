using Domain.Models;

namespace Application.Interfaces.ServiceInterfaces;

public interface IRoomService
    : IBaseService<Guid, Room>
{
    Task<ICollection<Reservation>> GetReservationsAsync(Guid roomId, CancellationToken ct);
    Task<ICollection<Reservation>> GetReservationsAsync(IEnumerable<Guid> roomsId, CancellationToken ct);

    // check if there is any reservation
    Task<bool> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        CancellationToken ct);

    // check if there is any reservation, but guestId is ignored, its mainly used for updating reservation
    Task<bool> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        Guid guestId, CancellationToken ct);
}