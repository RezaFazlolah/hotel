using Domain.Interface;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class User
    : IdentityUser<Guid>, IEntity<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
}