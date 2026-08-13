namespace Application.Rooms.Sorts;

public class RoomSortParameters
{
    public RoomSortBy SortBy { get; init; } = RoomSortBy.None;
    public bool IsAscending { get; init; } = true;
}