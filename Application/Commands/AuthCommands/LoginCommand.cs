using Application.DTOs.AuthDtos;
using Application.Result;
using Infrastructure;
using MediatR;

namespace Application.Commands.AuthCommands;

public class LoginCommand : IRequest<Result<AppUser>>
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}