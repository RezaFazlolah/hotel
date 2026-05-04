using Domain.Enums;

namespace Api.DTOs.AuthDtos;

public class RegisterByAdminCommandDto
{
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
}