namespace Application.Auth.Dtos;

public record LoggedinUserDto
 : UserDto
{
  public string Jwt { get; set; } = string.Empty;
}