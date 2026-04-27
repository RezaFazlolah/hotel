using Domain.Models;

namespace Domain.Repositories;

public interface IRoomRepository : IBaseRepository<Room, Guid>
{
    // roomId MUST NOT be updated
    Task<bool> IsRoomNumberUniqueAsync(Guid roomId, Guid hotelId, int roomNumber, CancellationToken cancellationToken);
}