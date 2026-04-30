namespace Api.DTOs.AuthDtos;

public class AppUserDto
{ 
     public Guid Id { get; set; }
     public string PhoneNumber { get; set; } = string.Empty;
     // public ICollection<string> Roles { get; set; } = [];
     public string Jwt { get; set; } = string.Empty;
}