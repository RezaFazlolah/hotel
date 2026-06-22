namespace Api.Dtos.HotelDtos;

public class HotelDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public float Rating { get; set; }
    // public ICollection<ManagerDto> Managers { get; set; } = [];
    // public ICollection<RoomDto> Rooms { get; set; } = [];
}