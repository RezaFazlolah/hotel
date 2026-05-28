using Application.Commands.AuthCommands;
using Application.Interfaces.ServiceInterfaces;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.AuthHandlers;

public class LoginHandler(IUserService userService, ITokenService tokenService)
    : IRequestHandler<Login, Result<string>>
{
    public async Task<Result<string>> Handle(Login request, CancellationToken cancellationToken)
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