using Application.Hotels.Sorts;

namespace Api.Dtos.HotelDtos;

// Future: use inheritance for GetAllHotelsQueryDto, GetAllRoomsQueryDto, GetAllReservationsQueryDto 
public class GetAllHotelsQueryDto
{
    // pagination
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    // filter
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? MinRating { get; set; }
    public decimal? MaxRating { get; set; }
    // sort
    public HotelSortBy? SortBy { get; set; }
    public bool? IsAscending { get; set; }
}