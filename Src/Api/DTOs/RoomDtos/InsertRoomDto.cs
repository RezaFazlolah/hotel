using SharedKernel.Enums;

namespace Api.DTOs.RoomDtos;

public class InsertRoomDto
{
    public required int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public required Guid HotelId { get; set; }
}