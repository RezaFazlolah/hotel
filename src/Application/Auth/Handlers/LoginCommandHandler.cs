using Application.Auth.Commands;
using Application.Auth.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Auth.Handlers;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtService jwtService,
    IMapper mapper)
    : IRequestHandler<LoginCommand, Result<LoggedinUserDto>>
{
    public async Task<Result<LoggedinUserDto>> Handle(
        LoginCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"user {request.PhoneNumber} login failed");

        var userResult = await userRepository.GetByPhoneNumberAsync(request.PhoneNumber, ct);
        if (!userResult.Succeeded)
            return Result<LoggedinUserDto>.Failure([rootError, new Error("user not found")], ResultCode.NotFound);
        var user = userResult.Value;

        var isPasswordCorrect = await userRepository.CheckPassword(user, request.Password);
        if (!isPasswordCorrect)
            return Result<LoggedinUserDto>.Failure([rootError, new Error("incorrect password")]);

        var jwtResult = await jwtService.GenerateJwt(user);
        if (!jwtResult.Succeeded)
            return Result<LoggedinUserDto>.Failure(jwtResult.Errors.Prepend(rootError));
        var jwt = jwtResult.Value;

        var rolesResult = await userRepository.GetRolesAsync(user, ct);
        if (!rolesResult.Succeeded)
            return Result<LoggedinUserDto>.Failure(rolesResult.Errors.Prepend(rootError));
        var roles = rolesResult.Value;

        var userDto = mapper.Map<LoggedinUserDto>(user) with { Roles = roles, Jwt = jwt };
        return Result<LoggedinUserDto>.Success(userDto);
    }
}