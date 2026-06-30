namespace Application.Hotels.Dtos;

public class HotelDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    
    // public IReadOnlyList<ManagerDto> Managers { get; set; } = [];
    // public IReadOnlyList<RoomDto> Rooms { get; set; } = [];
}