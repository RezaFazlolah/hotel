using Application.Auth.AuthCommands.AuthCommandRequests;
using Application.Auth.AuthDtos;
using Application.Result;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.AuthCommands.AuthCommandHandlers;

public class RregisterAuthHandler(UserManager<AppUser> userManager) : IRequestHandler<RegisterAuthCommand, Result<RegisterAuthDto>>
{
    public async Task<Result<RegisterAuthDto>> Handle(RegisterAuthCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber,
            cancellationToken);
        if (existingUser != null)
            return Result<RegisterAuthDto>.Failure($"user {existingUser.PhoneNumber} is already registered", 400);

        var user = new AppUser
        {
            PhoneNumber = request.PhoneNumber,
            UserName = request.PhoneNumber
        };
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return Result<RegisterAuthDto>.Failure("user register failed", 400);

        result = await userManager.AddToRoleAsync(user, "Guest");
        if (result.Succeeded)
        {
            var registerAuthDto = new RegisterAuthDto
            {
                Id = Guid.Parse(user.Id),
                PhoneNumber = user.PhoneNumber,
                Roles = ["Guest"]
            };
            return Result<RegisterAuthDto>.Success(registerAuthDto);
        }
        return Result<RegisterAuthDto>.Failure("user register failed", 400);
    }
}