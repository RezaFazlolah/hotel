namespace Application.Auth.Dtos;

public record LoggedinUserDto
 : BaseUserDto
{
  public string Jwt { get; init; } = string.Empty;
}