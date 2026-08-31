namespace Api.Dtos.RoomDtos;

public record UpdateRoomAsAdminCommandDto
    : UpdateRoomCommandDtoBase
{
    public required Guid HotelId { get; init; }
}
