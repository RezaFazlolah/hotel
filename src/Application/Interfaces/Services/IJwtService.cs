using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Services;

public interface IJwtService
{
    Task<Result<string>> GenerateJwt(User user);
}