using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Interfaces;

namespace Api.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserService
{
    public Guid? CurrentUserId
    {
        get
        {
            var currentUserId = httpContextAccessor?.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (currentUserId == null)
                return null;
            return Guid.TryParse(currentUserId, out var result) ? result : null;
        }
    }
}