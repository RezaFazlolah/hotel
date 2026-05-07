using Application.Models;
using MediatR;

namespace Application.Commands.AuthCommands;

public class LoginCommand : IRequest<Result<string>>
{
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
}