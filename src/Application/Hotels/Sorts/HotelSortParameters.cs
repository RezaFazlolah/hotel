using SharedKernel.Sorts;

namespace Application.Hotels.Sorts;

public class HotelSortParameters
    : BaseSortParameters
{
    public HotelSortBy HotelSortBy { get; init; }
}

public enum HotelSortBy
{
    Name,
    Address,
    Rating
}
