using SharedKernel.Filters;

namespace Application.Hotels.Filters;

public class HotelFilterParameters
    : BaseFilterParameters
{
    public string? Name { get; init; }
    public string? Address { get; init; }
    public decimal? MinRating { get; init; }
    public decimal? MaxRating { get; init; }
}