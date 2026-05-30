using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Api.Services;

public class CurrentUserRepository(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    : ICurrentUserRepository
{
    public Guid Id =>
        Guid.Parse(httpContextAccessor?.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub));

    public async Task<Result<IEnumerable<UserRole>>> GetRolesAsync(CancellationToken ct)
        => await userRepository.GetRolesAsync(Id, ct);
}