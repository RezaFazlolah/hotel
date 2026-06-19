using Application.Auth.Commands;
using Application.Interfaces.Repositories;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Auth.Handlers;

public class RegisterCommandHandler(IUserRepository userRepository)
    : IRequestHandler<RegisterCommand, Result<User>>
{
    public async Task<Result<User>> Handle(RegisterCommand request, CancellationToken ct)
    {
        var user = new User
        {
            PhoneNumber = request.PhoneNumber,
            UserName = request.PhoneNumber
        };

        if (!await userRepository.RoleExistsAsync(request.Role, ct))
            return Result<User>.Failure(new Error("role not found"));

        return await userRepository.InsertAsync(user, request.Password, ct);
    }
}