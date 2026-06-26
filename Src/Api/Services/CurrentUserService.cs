using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Models;
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

    public Result<User> User
    {
        get
        {
            var currentUserIdResult = Id;
            if (!currentUserIdResult.Succeeded)
                return Result<User>.Failure(currentUserIdResult.Errors);
            var currentUserId = currentUserIdResult.Value;

            return userRepository.GetByIdAsync(currentUserId, CancellationToken.None).Result;
        }
    }

    public Result<IEnumerable<UserRole>> Roles
    {
        get
        {
            var currentUserIdResult = this.Id;
            if (!currentUserIdResult.Succeeded)
                return Result<IEnumerable<UserRole>>.Failure(currentUserIdResult.Errors.Prepend(new Error($"get current user roles failed.")));
            var currentUserId = currentUserIdResult.Value;

            return userRepository.GetRolesAsync(currentUserId, CancellationToken.None).Result;
        }
    }

    public Result<(Guid id, User user, IEnumerable<UserRole> roles)> Info
    {
        get
        {
            var currentUserIdResult = this.Id;
            if (!currentUserIdResult.Succeeded)
                return Result<(Guid, User, IEnumerable<UserRole>)>.Failure(currentUserIdResult.Errors);
            var currentUserId = currentUserIdResult.Value;

            var currentUserResult = userRepository.GetByIdAsync(currentUserId, CancellationToken.None).Result;
            if (!currentUserResult.Succeeded)
                return Result<(Guid, User, IEnumerable<UserRole>)>.Failure(currentUserResult.Errors);
            var currentUser = currentUserResult.Value;
            
            var currentUserRolesResult = userRepository.GetRolesAsync(currentUserId, CancellationToken.None).Result;
            if(!currentUserRolesResult.Succeeded)
                return Result<(Guid, User, IEnumerable<UserRole>)>.Failure(currentUserRolesResult.Errors);
            var currentUserRoles = currentUserRolesResult.Value;
            
            return Result<(Guid, User, IEnumerable<UserRole>)>.Success((currentUserId, currentUser, currentUserRoles));
        }
    }
}