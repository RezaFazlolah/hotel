using SharedKernel.Enums;

namespace Api.Dtos.AuthDtos;

public class RegisterByAdminCommandDto
{
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
    public string FirstName{ get; init; }
    public string LastName { get; init; }
    public required UserRole Role { get; init; }
}