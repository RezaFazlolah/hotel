using Application.Commands.AuthCommands;
using Application.Result;
using Infrastructure;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.CommandHandlers.AuthCommandHandlers;

public class LoginCommandHandler(UserManager<AppUser> userManager, ITokenRepository tokenRepository)
    : IRequestHandler<LoginCommand, Result<AppUser>>
{
    public async Task<Result<AppUser>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber,
            cancellationToken: cancellationToken);
        if (user == null)
            return Result<AppUser>.Failure($"user not found", 404);
        var checkPassword = await userManager.CheckPasswordAsync(user, request.Password);
        if (checkPassword)
        {
            var roles = await userManager.GetRolesAsync(user);

            var jwt = tokenRepository.CreateJwt(user, roles.ToList());
            var response = new AppUser
            {
                Roles = roles,
                Id = Guid.Parse(user.Id),
                Jwt = jwt
            };
            return Result<AppUser>.Success(response);
        }

        return Result<AppUser>.Failure($"login failed", 400);
    }
}