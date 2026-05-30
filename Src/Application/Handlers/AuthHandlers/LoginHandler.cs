using Application.Interfaces.Repositories;
using Application.Requests.AuthRequests;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.AuthHandlers;

public class LoginHandler(IUserRepository userRepository, ITokenRepository tokenRepository)
    : IRequestHandler<Login, Result<string>>
{
    public async Task<Result<string>> Handle(Login request, CancellationToken cancellationToken)
    {
        var userResult = await userRepository.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);
        if (!userResult.Succeeded)
            return Result<string>.Failure(new Error("user not found"), ResultCode.NotFound);
        var user = userResult.Value;

        var passwordChecked = await userRepository.PasswordChecks(user, request.Password);
        return passwordChecked
            ? Result<string>.Success((await tokenRepository.GenerateJwt(user)).Value)
            : Result<string>.Failure(new Error($"incorrect password"));
    }
}