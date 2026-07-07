using SharedKernel.Enums;

namespace Api.Dtos.RoomDtos;

public class UpdateRoomCommandDto
{
    public required int Number { get; set; }
    public RoomType? Type { get; set; }
    public decimal? PricePerNight { get; set; }
    public Guid? HotelId { get; set; }
}
