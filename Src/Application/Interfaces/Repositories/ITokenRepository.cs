using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface ITokenRepository
{
    Task<Result<string>> GenerateJwt(User user);
}