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
            return Result<User>.Failure(new Error("role not found"));
        
        return await userService.InsertAsync(user, request.Password, ct);
    }
}