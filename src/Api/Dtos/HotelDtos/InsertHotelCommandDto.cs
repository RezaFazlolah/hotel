namespace Api.Dtos.HotelDtos;

public record InsertHotelCommandDto
{
    public required string Name { get; init; }
    public required string Address { get; init; }
    public float Rating { get; init; }
}
