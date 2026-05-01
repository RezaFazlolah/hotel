using Application.Commands.AuthCommands;
using Application.Models;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.CommandHandlers.AuthCommandHandlers;

public class LoginCommandHandler(IUserRepository userRepository)
    : IRequestHandler<LoginCommand, Result<AppUser>>
{
    public async Task<Result<AppUser>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);
        if (user == null)
            return Result<AppUser>.Failure(new Error("user not found"), 404);

        var passwordChecked = await userRepository.PasswordChecks(user, request.Password);
        if (!passwordChecked)
            return Result<AppUser>.Failure(new Error($"incorrect password"), 400);

        var loggedUser = new AppUser
        {
            User = user,
            Jwt = await userRepository.CreateJwt(user)
        };
        return Result<AppUser>.Success(loggedUser);
    }
}