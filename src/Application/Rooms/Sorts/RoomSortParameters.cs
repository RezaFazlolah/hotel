using Application.Common.Sorts;

namespace Application.Rooms.Sorts;

public class RoomSortParameters
    : BaseSortParameters
{
    public RoomSortBy SortBy { get; init; } = RoomSortBy.None;
}