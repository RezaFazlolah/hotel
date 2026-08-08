using Application.Common.Sorts;

namespace Application.Reservations.Sorts;

public class ReservationSortParameters
    : BaseSortParameters
{
    public ReservationSortBy SortBy { get; init; } =  ReservationSortBy.None;
}