using SharedKernel.Enums;

namespace Api.Dtos.RoomDtos;

public record UpdateRoomCommandDto
{
    public required int Number { get; init; }
    public RoomType? Type { get; init; }
    public decimal? PricePerNight { get; init; }
    public Guid? HotelId { get; init; }
}
