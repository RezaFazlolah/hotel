using SharedKernel.Enums;

namespace Application.Rooms.Dtos;

public record RoomDto
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init; }
    public int Number { get; init; }
    public RoomType Type { get; init; }
    public decimal PricePerNight { get; init; }
    // public HotelDto? Hotel { get; set; }
}