using System.Text.Json.Serialization;

namespace Api.Dtos.HotelDtos;

public record UpdateHotelBaseCommandDto
{
    public required string Name { get; init; }
    public required string Address { get; init; }
}
