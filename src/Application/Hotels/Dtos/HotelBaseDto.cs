namespace Application.Hotels.Dtos;

public record HotelBaseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public decimal Rating { get; init; }   
}