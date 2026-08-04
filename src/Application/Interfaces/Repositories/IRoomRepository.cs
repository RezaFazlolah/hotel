using Application.Rooms.Filters;
using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IRoomRepository
    : IBaseRepository<Guid, Room, RoomFilterParameters>
{
    Task<Result<IReadOnlyList<Room>>> GetAllByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct);

    Task<Result<bool>> RoomNumberExistsAsync(
        Guid hotelId,
        int roomNumber,
        CancellationToken ct);

    Task<Result<Guid>> GetHotelIdAsync(
        Guid roomId,
        CancellationToken ct);

    Task<Result<IReadOnlyList<Guid>>> GetAllIdsByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct);

    Task<Result<IReadOnlyList<Guid>>> GetAllIdsByHotelIdsAsync(
        IEnumerable<Guid> hotelIds,
        CancellationToken ct);
}