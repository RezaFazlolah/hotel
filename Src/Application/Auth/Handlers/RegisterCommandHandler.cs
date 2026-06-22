using Application.Auth.Commands;
using Application.Dtos.Auth;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Auth.Handlers;

public class RegisterCommandHandler(IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<RegisterCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(RegisterCommand request, CancellationToken ct)
    {
        var user = new User
        {
            PhoneNumber = request.PhoneNumber,
            UserName = request.PhoneNumber
        };

        if (!await userRepository.RoleExistsAsync(request.Role, ct))
            return Result<UserDto>.Failure(new Error("role not found"));

        var userResult = await userRepository.InsertAsync(user, request.Password, ct);
        return mapper.Map<Result<UserDto>>(userResult);
    }
}