namespace Api.Dtos.HotelDtos;

public record UpdateHotelCommandDto
{
    public string? Name { get; init; }
    public string? Address { get; init; }
    public float? Rating { get; init; }
    public Guid? ManagerId { get; init; }
    
}
