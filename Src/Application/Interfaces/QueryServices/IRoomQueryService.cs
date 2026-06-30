using Application.Rooms.Dtos;
using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.QueryServices;

public interface IRoomQueryService
    : IBaseQueryService<Room, RoomDto>
{
    Task<Result<IReadOnlyList<Guid>>> GetAllIdsByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct);

    Task<Result<IReadOnlyList<Guid>>> GetAllIdsByHotelIdsAsync(
        IEnumerable<Guid> hotelIds,
        CancellationToken ct);
}