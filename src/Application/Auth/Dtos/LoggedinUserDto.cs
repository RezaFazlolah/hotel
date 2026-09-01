namespace Application.Auth.Dtos;

public record LoggedinUserDto
 : UserDto
{
  public string Jwt { get; init; } = string.Empty;
}