using System.Text.Json.Serialization;

namespace Application.Hotels.Dtos;

public record HotelDto
    : BaseHotelDto
{
    [JsonPropertyOrder(1)]
    public Guid? ManagerId { get; init; }
}