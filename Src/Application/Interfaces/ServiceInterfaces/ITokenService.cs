using Domain.Models;

namespace Application.Interfaces.ServiceInterfaces;

public interface ITokenService
{
    Task<string?> GenerateJwt(User user);
}