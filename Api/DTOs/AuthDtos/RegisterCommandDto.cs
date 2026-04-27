namespace Api.DTOs.AuthDtos;

public class RegisterCommandDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
