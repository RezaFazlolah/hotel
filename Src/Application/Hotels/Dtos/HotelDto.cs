namespace Application.Hotels.Dtos;

public record HotelDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public decimal Rating { get; init; }
    
    // public IReadOnlyList<ManagerDto> Managers { get; set; } = [];
    // public IReadOnlyList<RoomDto> Rooms { get; set; } = [];
}