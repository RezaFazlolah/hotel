using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Api.Services;

public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor,
    IUserRepository userRepository)
    : ICurrentUserService
{
    public Result<Guid> Id =>
        Guid.TryParse(httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub),
            out var currentUserId)
            ? Result<Guid>.Success(currentUserId)
            : Result<Guid>.Failure(new Error("parsing current user ID failed."));

    public Result<IReadOnlyList<UserRole>> Roles
    {
        get
        {
            var roles = httpContextAccessor.HttpContext?.User
                .FindAll(ClaimTypes.Role)
                .Select(c => Enum.Parse<UserRole>(c.Value))
                .ToList();

            return roles is null
                ? Result<IReadOnlyList<UserRole>>.Failure(new Error("getting current user roles failed."))
                : Result<IReadOnlyList<UserRole>>.Success(roles);
        }
    }

    public async Task<Result<User>> GetUserAsync(CancellationToken ct)
    {
        var currentUserIdResult = Id;
        if (!currentUserIdResult.Succeeded)
            return Result<User>.Failure(currentUserIdResult.Errors);
        var currentUserId = currentUserIdResult.Value;

        return await userRepository.GetByIdAsync(currentUserId, CancellationToken.None);
    }

    public async Task<Result<(Guid id, User user, IReadOnlyList<UserRole> roles)>> GetUserInfoAsync(
        CancellationToken ct)
    {
        var currentUserIdResult = this.Id;
        if (!currentUserIdResult.Succeeded)
            return Result<(Guid, User, IReadOnlyList<UserRole>)>.Failure(currentUserIdResult.Errors);
        var currentUserId = currentUserIdResult.Value;

        var currentUserResult = await this.GetUserAsync(ct);
        if (!currentUserResult.Succeeded)
            return Result<(Guid, User, IReadOnlyList<UserRole>)>.Failure(currentUserResult.Errors);
        var currentUser = currentUserResult.Value;

        var currentUserRolesResult = this.Roles;
        if (!currentUserRolesResult.Succeeded)
            return Result<(Guid, User, IReadOnlyList<UserRole>)>.Failure(currentUserRolesResult.Errors);
        var currentUserRoles = currentUserRolesResult.Value;

        return Result<(Guid, User, IReadOnlyList<UserRole>)>.Success((currentUserId, currentUser, currentUserRoles));
    }
}