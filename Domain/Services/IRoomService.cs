using Domain.Models;

namespace Domain.Services;

public interface IRoomService
    : IBaseService<Guid, Room>
{
    /// <summary> roomId MUST NOT be updated </summary>
    Task<bool> IsRoomNumberUniqueAsync(Guid roomId, Guid hotelId, int roomNumber, CancellationToken cancellationToken);

    Task<ICollection<Reservation>> GetReservationsAsync(Guid roomId, CancellationToken ct);
    Task<ICollection<Reservation>> GetReservationsAsync(IEnumerable<Guid> roomsId, CancellationToken ct);
}