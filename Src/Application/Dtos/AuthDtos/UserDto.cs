namespace Application.Dtos.Auth;

public class UserDto
{
    public Guid Id { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;
}