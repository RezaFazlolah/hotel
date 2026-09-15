namespace Application.Hotels.Configurations;

public class HotelSettings
{
    public const string SectionName = "HotelSettings";
    public int MinRating { get; init; }
    public int MaxRating { get; init; }
    public int MaxNameLength { get; init; }
    public int MaxAddressLength { get; init; }
}