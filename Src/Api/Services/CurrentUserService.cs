using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Api.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    : ICurrentUserService
{
    public Result<Guid> Id =>
        Guid.TryParse(httpContextAccessor?.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub),
            out var currentUserId)
            ? Result<Guid>.Success(currentUserId)
            : Result<Guid>.Failure(new Error("current user ID parse failed."));

    public async Task<Result<IEnumerable<UserRole>>> GetRolesAsync(CancellationToken ct)
    {
        if (!Id.Succeeded)
            return Result<IEnumerable<UserRole>>.Failure(
                Id.Errors.Prepend(new Error("get current user's roles failed.")));
        return await userRepository.GetRolesAsync(Id.Value, ct);
    }
}