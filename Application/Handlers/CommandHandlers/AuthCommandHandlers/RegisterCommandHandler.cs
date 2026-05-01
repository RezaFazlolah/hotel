using Application.Commands.AuthCommands;
using Application.Models;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers.CommandHandlers.AuthCommandHandlers;

public class RegisterCommandHandler(UserManager<User> userManager)
    : IRequestHandler<RegisterCommand, Result<AppUser>>
{
    public async Task<Result<AppUser>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            PhoneNumber = request.PhoneNumber,
            UserName = request.PhoneNumber
        };
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new Error(e.Description));
            return Result<AppUser>.Failure(errors, 400);
        }

        result = await userManager.AddToRoleAsync(user, "Guest");
        if (result.Succeeded)
        {
            var registeredUser = new AppUser
            {
                User = user,
                Jwt = ""
            };
            return Result<AppUser>.Success(registeredUser);
        }

        return Result<AppUser>.Failure(new Error("user registration failed"), 400);
    }
}