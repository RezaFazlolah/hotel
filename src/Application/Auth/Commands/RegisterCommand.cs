using Application.Auth.Dtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Auth.Commands;

public record RegisterCommand
    : IRequest<Result<RegisteredUserDto>>
{
        public required string PhoneNumber { get; init; }
        public required string Password { get; init; }
        public required string FirstName { get; init; }
        public string? LastName { get; init; }
        public required UserRole Role { get; init; }
}