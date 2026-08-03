using Application.Auth.Commands;
using Application.Auth.Dtos;
using Application.Auth.Factories;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Auth.Handlers;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IMapper mapper)
    : IRequestHandler<RegisterCommand, Result<RegisteredUserDto>>
{
    public async Task<Result<RegisteredUserDto>> Handle(
        RegisterCommand request,
        CancellationToken ct)
    {
        var registeringUser = UserFactory.CreateUserFromRegisterCommand(request); 

        var userRegisterResult = await userRepository.InsertAsync(registeringUser, request.Password, ct);
        if(!userRegisterResult.Succeeded)
            return Result<RegisteredUserDto>.Failure(userRegisterResult.Errors.Prepend(new Error($"register user {request.PhoneNumber} failed.")));
        
        var registeredUserResult = await userRepository.GetByPhoneNumberAsync(request.PhoneNumber, ct);
        if(!registeredUserResult.Succeeded)
            return Result<RegisteredUserDto>.Failure(registeredUserResult.Errors.Prepend(new Error($"register user {request.PhoneNumber} failed.")));
        var registeredUser = registeredUserResult.Value;
        
        var addRoleResult = await userRepository.AddRoleAsync(registeredUser, request.Role, ct);
        if (!addRoleResult.Succeeded)
        {
            // future: use atomic DB transactions. if role is not added to user, registered user will be deleted.
            var userDeleteResult = await userRepository.DeleteAsync(registeredUser, ct);
            
            if (!userDeleteResult.Succeeded)
            {
                var errors = userDeleteResult.Errors.Select(e => e.ToString());
                var errorsAsString = string.Join(". ", errors);
                throw new Exception(
                    $"RegisterCommandHandler: user was saved to DB, but adding role to user failed, so i tried deleting user from DB, but i got error(s): {errorsAsString}");
            }
            return Result<RegisteredUserDto>.Failure(addRoleResult.Errors.Prepend(new Error($"register user {request.PhoneNumber} failed.")));
        }
        
        var registeredUserDto = mapper.Map<RegisteredUserDto>(registeredUser) with { Roles = [request.Role.ToString()] };
        return Result<RegisteredUserDto>.Success(registeredUserDto);
    }
}