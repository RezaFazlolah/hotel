using SharedKernel.Enums;

namespace Api.Dtos.AuthDtos;

public class RegisterByAdminCommandDto
{
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required UserRole Role { get; set; }
}