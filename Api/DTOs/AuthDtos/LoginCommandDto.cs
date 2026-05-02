using Domain.Enums;

namespace Api.DTOs.AuthDtos;

public class LoginCommandDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRoles UserRole { get; set; } = UserRoles.Guest;
}