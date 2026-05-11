using Application.Models;
using Domain.Models;
using MediatR;
using SharedKernel.Enums;

namespace Application.Commands.AuthCommands;

public class RegisterCommand : IRequest<Result<User>>
{
    public required string PhoneNumber { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
    public required UserRole Role { get; set; }
}