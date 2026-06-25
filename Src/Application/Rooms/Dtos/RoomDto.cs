using SharedKernel.Enums;

namespace Application.Rooms.Dtos;

public class RoomDto
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public int Number { get; set; }
    public RoomType Type { get; set; }

    public decimal PricePerNight { get; set; }
    // public HotelDto? Hotel { get; set; }
}