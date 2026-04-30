using Infrastructure;

namespace Application.Models;

public class AppUser
{
    public required User User { get; set; }
    public required string Jwt { get; set; }
}