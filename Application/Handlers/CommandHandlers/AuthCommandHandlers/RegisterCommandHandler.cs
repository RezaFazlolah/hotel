using Application.Commands.AuthCommands;
using Application.DTOs.AuthDtos;
using Application.Result;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.CommandHandlers.AuthCommandHandlers;

public class RegisterCommandHandler(UserManager<AppUser> userManager)
    : IRequestHandler<RegisterCommand, Result<RegisterDto>>
{
    public async Task<Result<RegisterDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber,
            cancellationToken);
        if (existingUser != null)
            return Result<RegisterDto>.Failure($"user {existingUser.PhoneNumber} is already registered", 400);

        var user = new AppUser
        {
            PhoneNumber = request.PhoneNumber,
            UserName = request.PhoneNumber
        };
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = new List<string>();
            foreach (var error in result.Errors)
                errors.Add(error.Description);

            return Result<RegisterDto>.Failure($"user register failed: {string.Join(", ", errors)}", 400);
        }

        result = await userManager.AddToRoleAsync(user, "Guest");
        if (result.Succeeded)
        {
            var registerAuthDto = new RegisterDto
            {
                Id = Guid.Parse(user.Id),
                PhoneNumber = user.PhoneNumber,
                Roles = ["Guest"]
            };
            return Result<RegisterDto>.Success(registerAuthDto);
        }
        return Result<RegisterDto>.Failure("user register failed", 400);
    }
}