using Application.Commands.AuthCommands;
using Application.Models;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.CommandHandlers.AuthCommandHandlers;

public class LoginCommandHandler(UserManager<User> userManager, IUserRepository userRepository)
    : IRequestHandler<LoginCommand, Result<AppUser>>
{
    public async Task<Result<AppUser>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber,
            cancellationToken: cancellationToken);
        if (user == null)
            return Result<AppUser>.Failure(new Error("user not found"), 404);

        var checkPassword = await userManager.CheckPasswordAsync(user, request.Password);
        if (checkPassword)
        {
            var roles = await userManager.GetRolesAsync(user);

            var jwt = userRepository.CreateJwt(user, roles.ToList());
            var loggedinUser = new AppUser
            {
                User = user,
                Jwt = jwt
            };
            return Result<AppUser>.Success(loggedinUser);
        }

        return Result<AppUser>.Failure(new Error($"incorrect password"), 400);
    }
}