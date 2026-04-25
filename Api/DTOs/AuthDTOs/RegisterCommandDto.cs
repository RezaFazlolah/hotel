namespace Api.DTOs.AuthDTOs;

public class RegisterCommandDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
