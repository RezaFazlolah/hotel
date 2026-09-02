using System.Text.Json.Serialization;

namespace Application.Hotels.Dtos;

public record HotelDto
    : HotelBaseDto
{
    [JsonPropertyOrder(1)]
    public Guid? ManagerId { get; init; }
}