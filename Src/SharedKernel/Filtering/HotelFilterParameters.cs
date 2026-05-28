namespace SharedKernel.Filtering;

public class HotelFilterParameters : BaseFilterParameters
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? MinRating { get; set; }
    public decimal? MaxRating { get; set; }
}