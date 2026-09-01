using SharedKernel.Enums;

namespace Api.Dtos.RoomDtos;

public record UpdateRoomCommandBaseDto
{
    public required int Number { get; init; }
    public required RoomType Type { get; init; }
    public required decimal PricePerNight { get; init; }
}