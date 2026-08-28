using Application.Hotels.Sorts;

namespace Api.Dtos.HotelDtos;

// Future: use inheritance for GetAllHotelsQueryDto, GetAllRoomsQueryDto, GetAllReservationsQueryDto 
public record GetAllHotelsQueryDto
{
    // filter
    public string? Name { get; init; }
    public string? Address { get; init; }
    public decimal? MinRating { get; init; }
    public decimal? MaxRating { get; init; }
    // sort
    public HotelSortBy? SortBy { get; init; }
    public bool? IsAscending { get; init; }
    // pagination
    public int? PageNumber { get; init; }
    public int? PageSize { get; init; }
}