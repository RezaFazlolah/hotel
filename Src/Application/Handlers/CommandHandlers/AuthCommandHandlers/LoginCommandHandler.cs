using Application.Commands.AuthCommands;
using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.CommandHandlers.AuthCommandHandlers;

public class LoginCommandHandler(IUserService userService, ITokenService tokenService)
    : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var userResult = await userService.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);
        if (!userResult.Succeeded)
            return Result<string>.Failure(new Error("user not found"), ResultCode.NotFound);
        var user = userResult.Value;
        
        var passwordChecked = await userService.PasswordChecks(user, request.Password);
        return passwordChecked
            ? Result<string>.Success((await tokenService.GenerateJwt(user)).Value)
            : Result<string>.Failure(new Error($"incorrect password"));
    }
}