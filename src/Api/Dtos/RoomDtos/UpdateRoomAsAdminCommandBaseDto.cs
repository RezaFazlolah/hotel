namespace Api.Dtos.RoomDtos;

public record UpdateRoomAsAdminCommandBaseDto
    : UpdateRoomBaseCommandDto
{
    public required Guid HotelId { get; init; }
}
