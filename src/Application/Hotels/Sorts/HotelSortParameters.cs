namespace Application.Hotels.Sorts;

public class HotelSortParameters
{
    public HotelSortBy SortBy { get; init; } = HotelSortBy.None;
    public bool IsAscending { get; init; } = true;
}