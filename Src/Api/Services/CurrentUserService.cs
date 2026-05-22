using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Interfaces;
using Application.Interfaces.ServiceInterfaces;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Api.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserService userService)
    : ICurrentUserService
{
    public Guid Id =>
        Guid.Parse(httpContextAccessor?.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub));

    public async Task<Result<IEnumerable<UserRole>>> GetRolesAsync(CancellationToken ct) 
        => await userService.GetRolesAsync(Id, ct);
}