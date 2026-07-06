using Application.Auth.Dtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Auth.Commands;

public record RegisterCommand(
    string PhoneNumber,
    string Password,
    string FirstName,
    string LastName,
    UserRole Role)
    : IRequest<Result<RegisteredUserDto>>;