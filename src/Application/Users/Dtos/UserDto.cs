using SharedKernel.Enums;

namespace Application.Users.Dtos;

public class UserDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ICollection<UserRole> Roles { get; set; } = [];
}