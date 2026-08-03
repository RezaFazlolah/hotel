namespace Api.Dtos.AuthDtos;

public class RegisterCommandDto
{
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
}