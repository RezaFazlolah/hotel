using System.Text.Json.Serialization;

namespace Application.Auth.Dtos;

public record ManagerDto
    : UserDto
{
    [JsonPropertyOrder(1)] 
    public Guid? HotelId { get; init; }
}