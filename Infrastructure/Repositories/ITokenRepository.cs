using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public interface ITokenRepository
{
    string CreateJwt(User user, ICollection<string> roles);
}