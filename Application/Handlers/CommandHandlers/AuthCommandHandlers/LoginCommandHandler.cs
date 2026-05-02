using Application.Commands.AuthCommands;
using Application.Models;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.CommandHandlers.AuthCommandHandlers;

public class LoginCommandHandler(IUserRepository userRepository)
    : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);
        if (user == null)
            return Result<string>.Failure(new Error("user not found"), 404);

        var passwordChecked = await userRepository.PasswordChecks(user, request.Password);
        return passwordChecked
            ? Result<string>.Success(await userRepository.GenerateJwt(user))
            : Result<string>.Failure(new Error($"incorrect password"), 400);
    }
}