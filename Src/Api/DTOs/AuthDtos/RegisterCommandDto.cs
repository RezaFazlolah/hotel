namespace Api.DTOs.AuthDtos;

public class RegisterCommandDto
{
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
}