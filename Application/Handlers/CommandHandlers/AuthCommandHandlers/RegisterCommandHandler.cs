using Application.Commands.AuthCommands;
using Application.Models;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            var errors = new List<Error>();
            foreach (var error in result.Errors)
                errors.Add(new Error(error.Description));

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