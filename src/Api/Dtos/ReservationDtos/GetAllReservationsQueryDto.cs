using Application.Reservations.Sorts;
using SharedKernel.Enums;

namespace Api.Dtos.ReservationDtos;

// Future: use inheritance for GetAllHotelsQueryDto, GetAllRoomsQueryDto, GetAllReservationsQueryDto 
public record GetAllReservationsQueryDto
{
    // filter
    public DateTimeOffset? MinCheckInDate { get; init; }
    public DateTimeOffset? MaxCheckInDate { get; init; }
    public DateTimeOffset? MinCheckOutDate { get; init; }
    public DateTimeOffset? MaxCheckOutDate { get; init; }
    public decimal? MinTotalPrice { get; init; }
    public decimal? MaxTotalPrice { get; init; }
    public ReservationStatus? Status { get; init; }
    // sort
    public ReservationSortBy? SortBy { get; init; }
    public bool? IsAscending { get; init; }
    // pagination
    public int? PageNumber { get; init; }
    public int? PageSize { get; init; }
}