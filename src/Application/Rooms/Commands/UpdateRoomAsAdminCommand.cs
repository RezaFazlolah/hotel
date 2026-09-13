namespace Application.Rooms.Commands;

public record UpdateRoomAsAdminCommand
    : UpdateRoomBaseCommand
{
    public required Guid HotelId { get; init; }
}