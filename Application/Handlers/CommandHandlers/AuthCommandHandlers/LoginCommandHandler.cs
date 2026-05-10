using Application.Commands.AuthCommands;
using Application.Models;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.CommandHandlers.AuthCommandHandlers;

public class LoginCommandHandler(IUserService userService)
    : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);
        if (user == null)
            return Result<string>.Failure(new Error("user not found"), 404);

        var passwordChecked = await userService.PasswordChecks(user, request.Password);
        return passwordChecked
            ? Result<string>.Success(await userService.GenerateJwt(user))
            : Result<string>.Failure(new Error($"incorrect password"), 400);
    }
}