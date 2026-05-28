using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Commands.AuthCommands;

public class Register : IRequest<Result<User>>
{
    public required string PhoneNumber { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
    public required UserRole Role { get; set; }
}