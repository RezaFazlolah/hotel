using Application.Auth.Commands;
using Application.Interfaces.Repositories;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Auth.Handlers;

public class LoginCommandHandler(IUserRepository userRepository, ITokenRepository tokenRepository)
    : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
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