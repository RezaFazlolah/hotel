using Application.Interfaces.Repositories;
using Application.Requests.AuthRequests;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.AuthHandlers;

public class RegisterHandler(IUserRepository userRepository)
    : IRequestHandler<Register, Result<User>>
{
    public async Task<Result<User>> Handle(Register request, CancellationToken ct)
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