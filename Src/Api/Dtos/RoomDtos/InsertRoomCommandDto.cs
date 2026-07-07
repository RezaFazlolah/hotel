using SharedKernel.Enums;

namespace Api.Dtos.RoomDtos;

public class InsertRoomCommandDto
{
    public required int Number { get; set; }
    public required RoomType Type { get; set; }
    public required decimal PricePerNight { get; set; }
    public required Guid HotelId { get; set; }
}