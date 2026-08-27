using Application.Rooms.Dtos;
using Application.Rooms.Filters;
using Application.Rooms.Sorts;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.QueryServices;

public interface IRoomQueryService
    : IQueryServiceBase<RoomDto>
{
    Task<Result<PagedResult<RoomDto>>> GetAllAsync(
        RoomFilterParameters? hotelFilterParameters,
        RoomSortParameters? hotelSortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<RoomDto>>> GetAllByHotelAsync(
        Guid hotelId,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<RoomDto>>> GetAllByManagerAsync(
        Guid managerId,
        RoomFilterParameters? filterParameters,
        RoomSortParameters sortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);
}