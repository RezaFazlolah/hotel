using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public interface ITokenRepository
{
    string CreateJwt(AppUser user, ICollection<string> roles);
}