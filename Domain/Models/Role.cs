using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class Role
    : IdentityRole<Guid>
{
    public Role() : base()
    {
    }

    public Role(string role) : base(role)
    {
    }
}