using Application.Commands.AuthCommands;
using Application.Models;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers.CommandHandlers.AuthCommandHandlers;

public class RegisterCommandHandler(IUserRepository userRepository)
    : IRequestHandler<RegisterCommand, Result<AppUser>>
{
    public async Task<Result<AppUser>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            PhoneNumber = request.PhoneNumber,
            UserName = request.PhoneNumber
        };

        var result = await userRepository.RegisterAsync(user, request.Password, request.Role, cancellationToken);

        return result.Succeeded
            ? Result<AppUser>.Success(new AppUser
            {
                User = user,
                Jwt = ""
            })
            : Result<AppUser>.Failure(result.Errors.Select(e => new Error(e.Description)).ToList(), 400);
    }
}