using Api.DTOs.HotelDtos;
using Domain.Enums;

namespace Api.DTOs.RoomDtos;

public class RoomDto
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public HotelDto? HotelDto { get; set; }
}