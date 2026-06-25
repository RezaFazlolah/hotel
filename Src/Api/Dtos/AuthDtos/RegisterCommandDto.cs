namespace Api.Dtos.AuthDtos;

public class RegisterCommandDto
{
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}