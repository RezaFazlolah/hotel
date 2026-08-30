namespace Api.Dtos.HotelDtos;

public record UpdateHotelCommandDto
{
    public required string Name { get; init; }
    public required string Address { get; init; }
    public required decimal Rating { get; init; }
}
