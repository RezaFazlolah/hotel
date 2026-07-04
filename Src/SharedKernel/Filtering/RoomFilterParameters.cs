using SharedKernel.Enums;

namespace SharedKernel.Filtering;

public class RoomFilterParameters
    : BaseFilterParameters
{
    public int? MinNumber { get; set; }
    public int? MaxNumber { get; set; }
    public RoomType? Type { get; set; }
    public decimal? MinPricePerNight { get; set; }
    public decimal? MaxPricePerNight { get; set; }
}