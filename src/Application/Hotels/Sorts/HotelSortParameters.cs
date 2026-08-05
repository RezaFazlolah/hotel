using SharedKernel.Sorts;

namespace Application.Hotels.Sorts;

public class HotelSortParameters
    : BaseSortParameters
{
    public HotelSortBy SortBy { get; init; } =  HotelSortBy.None;
}

