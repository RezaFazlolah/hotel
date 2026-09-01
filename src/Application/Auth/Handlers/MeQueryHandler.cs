using Application.Auth.Dtos;
using Application.Auth.Queries;
using Application.Interfaces.Services;
using AutoMapper;
using MediatR;
using SharedKernel.Common;

namespace Application.Auth.Handlers;

public class MeQueryHandler(
    ICurrentUserService currentUserService,
    IMapper mapper)
    : IRequestHandler<MeQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(
        MeQuery request,
        CancellationToken ct)
    {
        var rootError = new Error("get current user info failed");
        
        var userResult = await currentUserService.GetCurrentUserAsync(ct);
        if(!userResult.Succeeded)
            return Result<UserDto>.Failure(userResult.Errors.Prepend(rootError));
        var user = userResult.Value;
        
        var rolesResult = currentUserService.Roles;
        if(!rolesResult.Succeeded)
            return Result<UserDto>.Failure(rolesResult.Errors.Prepend(rootError));
        var roles = rolesResult.Value;
        
        var userDto = mapper.Map<UserDto>(user) with {Roles = roles};
        return Result<UserDto>.Success(userDto);
    }
}