using Application.Commands.AuthCommands;
using Application.DTOs.AuthDtos;
using Application.Result;
using Infrastructure;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.CommandHandlers.AuthCommandHandlers;

public class LoginCommandHandler(UserManager<AppUser> userManager, ITokenRepository tokenRepository)
    : IRequestHandler<LoginCommand, Result<LoginDto>>
{
    public async Task<Result<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber,
            cancellationToken: cancellationToken);
        if (user == null)
            return Result<LoginDto>.Failure($"user not found", 404);
        var checkPassword = await userManager.CheckPasswordAsync(user, request.Password);
        if (checkPassword)
        {
            var roles = await userManager.GetRolesAsync(user);

            var jwt = tokenRepository.CreateJwt(user, roles.ToList());
            var response = new LoginDto
            {
                Roles = roles,
                Id = Guid.Parse(user.Id),
                Jwt = jwt
            };
            return Result<LoginDto>.Success(response);
        }

        return Result<LoginDto>.Failure($"login failed", 400);
    }
}