namespace Api.Dtos.HotelDtos;

public record UpdateHotelAsManagerCommandDto()
{
    public required string Name { get; init; }
    public required string Address { get; init; }
}