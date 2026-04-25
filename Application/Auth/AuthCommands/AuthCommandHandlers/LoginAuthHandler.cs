using Application.Auth.AuthCommands.AuthCommandRequests;
using Application.Auth.AuthDtos;
using Application.Result;
using Infrastructure;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.AuthCommands.AuthCommandHandlers;

public class LoginAuthHandler(UserManager<AppUser> userManager, ITokenRepository tokenRepository)
    : IRequestHandler<LoginAuthCommand, Result<LoginAuthDto>>
{
    public async Task<Result<LoginAuthDto>> Handle(LoginAuthCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber,
            cancellationToken: cancellationToken);
        if (user == null)
            return Result<LoginAuthDto>.Failure($"user not found", 404);
        var checkPassword = await userManager.CheckPasswordAsync(user, request.Password);
        if (checkPassword)
        {
            var roles = await userManager.GetRolesAsync(user);

            var jwt = tokenRepository.CreateJwt(user, roles.ToList());
            var response = new LoginAuthDto
            {
                Roles = roles,
                Id = Guid.Parse(user.Id),
                Jwt = jwt
            };
            return Result<LoginAuthDto>.Success(response);
        }

        return Result<LoginAuthDto>.Failure($"login failed", 400);
    }
}