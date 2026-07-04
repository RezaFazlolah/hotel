using SharedKernel.Enums;

namespace SharedKernel.Filtering;

public class ReservationFilterParameters
    : BaseFilterParameters
{
    public DateTimeOffset? MinCheckInDate { get; set; }
    public DateTimeOffset? MaxCheckInDate { get; set; }
    public DateTimeOffset? MinCheckOutDate { get; set; }
    public DateTimeOffset? MaxCheckOutDate { get; set; }
    public decimal? MinTotalPrice { get; set; }
    public decimal? MaxTotalPrice { get; set; }
    public ReservationStatus? Status { get; set; }
}