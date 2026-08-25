using SharedKernel.Enums;

namespace Api.Dtos.RoomDtos;

public record CreateRoomCommandDto
{
    public required int Number { get; init; }
    public required RoomType Type { get; init; }
    public required decimal PricePerNight { get; init; }
    public required Guid HotelId { get; init; }
}