namespace Api.Dtos.RoomDtos;

public record UpdateRoomAsAdminCommandBaseDto
    : UpdateRoomCommandBaseDto
{
    public required Guid HotelId { get; init; }
}
