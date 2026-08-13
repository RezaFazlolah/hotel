namespace Application.Reservations.Sorts;

public class ReservationSortParameters
{
    public ReservationSortBy SortBy { get; init; } =  ReservationSortBy.None;
    public bool IsAscending { get; init; } = true;
}