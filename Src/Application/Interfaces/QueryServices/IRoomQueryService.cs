using Application.Rooms.Dtos;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.QueryServices;

public interface IRoomQueryService
    : IBaseQueryService<Room, RoomDto>
{
    Task<Result<PagedResult<RoomDto>>> GetAllByHotelIdAsync(
        Guid hotelId,
        PaginationParameters paginationParameters,
        CancellationToken ct);
}