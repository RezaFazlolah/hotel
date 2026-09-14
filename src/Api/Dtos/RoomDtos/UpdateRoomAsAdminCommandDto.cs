namespace Api.Dtos.RoomDtos;

public record UpdateRoomAsAdminCommandDto
    : UpdateRoomAsManagerCommandDto
{
    public required Guid HotelId { get; init; }
}
