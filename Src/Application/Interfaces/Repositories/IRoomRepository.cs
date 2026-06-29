using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IRoomRepository
    : IBaseRepository<Guid, Room>
{
    Task<Result<ICollection<Room>>> GetRoomsByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct);
    
    Task<Result<bool>> RoomNumberExistsAsync(
        Guid hotelId,
        int roomNumber,
        CancellationToken ct);
}