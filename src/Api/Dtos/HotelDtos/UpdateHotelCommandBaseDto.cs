namespace Api.Dtos.HotelDtos;

public record UpdateHotelCommandBaseDto
{
    public required string Name { get; init; }
    public required string Address { get; init; }
}
