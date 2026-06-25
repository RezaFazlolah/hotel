using Application.Auth.Commands;
using Application.Auth.Dtos;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Auth.Handlers;

public class LoginCommandHandler(
    IUserRepository userRepository,
    ITokenRepository tokenRepository,
    IMapper mapper)
    : IRequestHandler<LoginCommand, Result<LoggedinUserDto>>
{
    public async Task<Result<LoggedinUserDto>> Handle(LoginCommand request, CancellationToken ct)
    {
        var userResult = await userRepository.GetByPhoneNumberAsync(request.PhoneNumber, ct);
        if (!userResult.Succeeded)
            return Result<LoggedinUserDto>.Failure(new Error($"user {request.PhoneNumber} login failed. user not found"),
                ResultCode.NotFound);
        var user = userResult.Value;

        var isPasswordCorrect = await userRepository.CheckPassword(user, request.Password);
        if (!isPasswordCorrect)
            return Result<LoggedinUserDto>.Failure(new Error($"user {request.PhoneNumber} login failed. incorrect password."));

        var jwtResult = await tokenRepository.GenerateJwt(user);
        if (!jwtResult.Succeeded)
            return Result<LoggedinUserDto>.Failure(
                jwtResult.Errors.Prepend(new Error($"user {request.PhoneNumber} login failed.")));
        var jwt = jwtResult.Value;

        var rolesResult = await userRepository.GetRolesAsync(user, ct);
        if (!rolesResult.Succeeded)
            return Result<LoggedinUserDto>.Failure(
                rolesResult.Errors.Prepend(new Error($"user {request.PhoneNumber} login failed.")));
        var roles = rolesResult.Value.Select(r => r.ToString()).ToArray();
        
        var userDto = mapper.Map<LoggedinUserDto>(user) with {Roles = roles, Jwt = jwt};
        return Result<LoggedinUserDto>.Success(userDto);
    }
}