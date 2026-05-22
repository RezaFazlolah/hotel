using Api.DTOs.RoomDtos;

namespace Api.DTOs.HotelDtos;

public class HotelDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public float Rating { get; set; }
    // public ICollection<Manager> Managers { get; set; } = [];
    // public ICollection<Room> Rooms { get; set; } = [];
}