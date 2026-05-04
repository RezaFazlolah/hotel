namespace Api.DTOs.AuthDtos;

public class UserDto
{
    public Guid Id { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;
    // public ICollection<string> Roles { get; set; } = [];
}