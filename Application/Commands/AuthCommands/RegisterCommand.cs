using Application.Models;
using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.Commands.AuthCommands;

public class RegisterCommand : IRequest<Result<User>>
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public required string Role { get; set; }
}