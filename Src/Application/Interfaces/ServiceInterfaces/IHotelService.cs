using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.ServiceInterfaces;

public interface IHotelService
    : IBaseService<Guid, Hotel>
{
    Task<Result<ICollection<Room>>> GetRoomsAsync(Guid hotelId, CancellationToken ct);
    Task<Result<ICollection<Room>>> GetRoomsAsync(IEnumerable<Guid> hotelsId, CancellationToken ct);
    Task<Result<ICollection<Guid>>> GetRoomsIdAsync(Guid hotelId, CancellationToken ct);
    Task<Result<ICollection<Guid>>> GetRoomsIdAsync(IEnumerable<Guid> hotelsId, CancellationToken ct);
    Task<Result<ICollection<Reservation>>> GetReservationsAsync(Guid hotelId, CancellationToken ct);
    Task<Result<ICollection<Reservation>>> GetReservationsAsync(IEnumerable<Guid> hotelsId, CancellationToken ct);
    Task<Result<bool>> RoomNumberExistsAsync(int roomNumber, Guid hotelId, CancellationToken cancellationToken);
}