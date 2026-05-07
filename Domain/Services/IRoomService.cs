using Domain.Models;

namespace Domain.Services;

public interface IRoomService 
    : IBaseService<Room, Guid>
{
    // roomId MUST NOT be updated
    Task<bool> IsRoomNumberUniqueAsync(Guid roomId, Guid hotelId, int roomNumber, CancellationToken cancellationToken);
}