using Application.Rooms.Dtos;
using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Services.Query;

public interface IRoomQueryService
    : IBaseQueryService<Room, RoomDto>
{
    Task<Result<ICollection<Guid>>> GetRoomsIdByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct);

    Task<Result<ICollection<Guid>>> GetRoomsIdByHotelIdsAsync(
        IEnumerable<Guid> hotelIds,
        CancellationToken ct);
}