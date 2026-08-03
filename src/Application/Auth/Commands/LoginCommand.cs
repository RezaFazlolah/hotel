using Application.Auth.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Auth.Commands;

public record LoginCommand
    : IRequest<Result<LoggedinUserDto>>
{
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
}