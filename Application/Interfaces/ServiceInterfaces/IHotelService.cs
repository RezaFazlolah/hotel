using Domain.Models;

namespace Application.Interfaces.ServiceInterfaces;

public interface IHotelService
    : IBaseService<Guid, Hotel>
{
    Task<ICollection<Room>> GetRoomsAsync(Guid hotelId, CancellationToken ct);
    Task<ICollection<Room>> GetRoomsAsync(IEnumerable<Guid> hotelsId, CancellationToken ct);
    Task<ICollection<Guid>> GetRoomsIdAsync(Guid hotelId, CancellationToken ct);
    Task<ICollection<Guid>> GetRoomsIdAsync(IEnumerable<Guid> hotelsId, CancellationToken ct);
    Task<ICollection<Reservation>> GetReservationsAsync(Guid hotelId, CancellationToken ct);
    Task<ICollection<Reservation>> GetReservationsAsync(IEnumerable<Guid> hotelsId, CancellationToken ct);
    Task<bool> RoomNumberExistsAsync(int roomNumber, Guid hotelId, CancellationToken cancellationToken);
}