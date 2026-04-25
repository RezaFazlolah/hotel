namespace Application.DTOs.AuthDtos;

public class RegisterDto
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public ICollection<string> Roles { get; set; } = [];
}