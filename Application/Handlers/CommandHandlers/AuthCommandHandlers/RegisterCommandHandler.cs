using Application.Commands.AuthCommands;
using Application.Interfaces.ServiceInterfaces;
using Application.Models;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers.CommandHandlers.AuthCommandHandlers;

public class RegisterCommandHandler(IUserService userService)
    : IRequestHandler<RegisterCommand, Result<User>>
{
    public async Task<Result<User>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            PhoneNumber = request.PhoneNumber,
            UserName = request.PhoneNumber
        };

        var result = await userService.RegisterAsync(user, request.Password, request.Role, cancellationToken);
        return result.Succeeded
            ? Result<User>.Success(await userService.GetByPhoneNumberAsync(user.PhoneNumber, cancellationToken), 201)
            : Result<User>.Failure(result.Errors.Select(e => new Error(e.Description)).ToList(), 400);
    }
}