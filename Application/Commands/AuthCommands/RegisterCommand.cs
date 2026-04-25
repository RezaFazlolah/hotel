using Application.DTOs.AuthDtos;
using Application.Result;
using MediatR;

namespace Application.Commands.AuthCommands;

public class RegisterCommand : IRequest<Result<RegisterDto>>
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
