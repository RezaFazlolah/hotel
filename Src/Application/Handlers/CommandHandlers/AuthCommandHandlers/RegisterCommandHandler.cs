using Application.Commands.AuthCommands;
using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Common;

namespace Application.Handlers.CommandHandlers.AuthCommandHandlers;

public class RegisterCommandHandler(IUserService userService)
    : IRequestHandler<RegisterCommand, Result<User>>
{
    public async Task<Result<User>> Handle(RegisterCommand request, CancellationToken ct)
    {
        var user = new User
        {
            PhoneNumber = request.PhoneNumber,
            UserName = request.PhoneNumber
        };

        if (!await userService.RoleExistsAsync(request.Role, ct))
            return Result<User>.Failure(new Error("role not found"), 400);
        
        var result = await userService.InsertAsync(user, request.Password, ct);
        return result == null
            ? Result<User>.Failure(new Error("user registed failed"), 400)
            : Result<User>.Success(result, 201);
    }
}