using Application.Rooms.Sorts;
using SharedKernel.Enums;

namespace Api.Dtos.RoomDtos;

// Future: use inheritance for GetAllHotelsQueryDto, GetAllRoomsQueryDto, GetAllReservationsQueryDto 
public class GetAllRoomsQueryDto
{
    // pagination
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    // filter
    public int? MinNumber { get; set; }
    public int? MaxNumber { get; set; }
    public RoomType? Type { get; set; }
    public decimal? MinPricePerNight { get; set; }
    public decimal? MaxPricePerNight { get; set; }
    // sort
    public RoomSortBy? SortBy { get; set; }
    public bool? IsAscending { get; set; }
}