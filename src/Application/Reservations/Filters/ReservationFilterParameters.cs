using Application.Common.Filters;
using SharedKernel.Enums;

namespace Application.Reservations.Filters;

public class ReservationFilterParameters
    : BaseFilterParameters
{
    public DateTimeOffset? MinCheckInDate { get; init; }
    public DateTimeOffset? MaxCheckInDate { get; init; }
    public DateTimeOffset? MinCheckOutDate { get; init; }
    public DateTimeOffset? MaxCheckOutDate { get; init; }
    public decimal? MinTotalPrice { get; init; }
    public decimal? MaxTotalPrice { get; init; }
    public ReservationStatus? Status { get; init; }
}