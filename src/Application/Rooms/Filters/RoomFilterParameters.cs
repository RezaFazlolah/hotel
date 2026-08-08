using Application.Common.Filters;
using SharedKernel.Enums;

namespace Application.Rooms.Filters;

public class RoomFilterParameters
    : BaseFilterParameters
{
    public int? MinNumber { get; init; }
    public int? MaxNumber { get; init; }
    public RoomType? Type { get; init; }
    public decimal? MinPricePerNight { get; init; }
    public decimal? MaxPricePerNight { get; init; }
}