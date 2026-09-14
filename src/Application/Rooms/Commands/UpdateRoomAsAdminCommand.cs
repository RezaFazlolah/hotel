namespace Application.Rooms.Commands;

public record UpdateRoomAsAdminCommand
    : UpdateRoomAsManagerCommand
{
    public required Guid HotelId { get; init; }
}