using System.Text.Json.Serialization;

namespace Application.Auth.Dtos;

public record LoggedinUserDto
    : UserDto
{
    [JsonPropertyOrder(1)] 
    public string Jwt { get; init; } = string.Empty;
}