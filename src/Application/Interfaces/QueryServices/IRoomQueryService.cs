using Application.Rooms.Dtos;
using Application.Rooms.Filters;
using Application.Rooms.Sorts;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.QueryServices;

public interface IRoomQueryService
    : IBaseQueryService<RoomDto>
{
    Task<Result<PagedResult<RoomDto>>> GetAllAsync(
        RoomFilterParameters? hotelFilterParameters,
        RoomSortParameters? hotelSortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);
    
    Task<Result<PagedResult<RoomDto>>> GetAllByHotelIdAsync(
        Guid hotelId,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<RoomDto>>> GetAllByManagerIdAsync(
        Guid managerId,
        PaginationParameters paginationParameters,
        CancellationToken ct);
}