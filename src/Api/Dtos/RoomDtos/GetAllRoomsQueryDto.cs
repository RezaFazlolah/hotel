using Application.Rooms.Sorts;
using SharedKernel.Enums;

namespace Api.Dtos.RoomDtos;

// Future: use inheritance for GetAllHotelsQueryDto, GetAllRoomsQueryDto, GetAllReservationsQueryDto 
public record GetAllRoomsQueryDto
{
    // filter
    public int? MinNumber { get; init; }
    public int? MaxNumber { get; init; }
    public RoomType? Type { get; init; }
    public decimal? MinPricePerNight { get; init; }
    public decimal? MaxPricePerNight { get; init; }
    // sort
    public RoomSortBy? SortBy { get; init; }
    // pagination
    public int? PageNumber { get; init; }
    public int? PageSize { get; init; }public bool? IsAscending { get; init; }
}