using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.ServiceInterfaces;

public interface ITokenService
{
    Task<Result<string>> GenerateJwt(User user);
}