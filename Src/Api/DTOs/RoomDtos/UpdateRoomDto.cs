using SharedKernel.Enums;

namespace Api.DTOs.RoomDtos;

public class UpdateRoomDto
{
    public int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public Guid? HotelId { get; set; }
}
