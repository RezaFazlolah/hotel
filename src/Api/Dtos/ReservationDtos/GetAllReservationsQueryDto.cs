using Application.Reservations.Sorts;
using SharedKernel.Enums;

namespace Api.Dtos.ReservationDtos;

// Future: use inheritance for GetAllHotelsQueryDto, GetAllRoomsQueryDto, GetAllReservationsQueryDto 
public class GetAllReservationsQueryDto
{
    // pagination
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    // filter
    public DateTimeOffset? MinCheckInDate { get; set; }
    public DateTimeOffset? MaxCheckInDate { get; set; }
    public DateTimeOffset? MinCheckOutDate { get; set; }
    public DateTimeOffset? MaxCheckOutDate { get; set; }
    public decimal? MinTotalPrice { get; set; }
    public decimal? MaxTotalPrice { get; set; }
    public ReservationStatus? Status { get; set; }
    // sort
    public ReservationSortBy? SortBy { get; set; }
    public bool? IsAscending { get; set; }
}