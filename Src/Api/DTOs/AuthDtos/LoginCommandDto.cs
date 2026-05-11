namespace Api.DTOs.AuthDtos;

public class LoginCommandDto
{
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
}