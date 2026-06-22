using Application.Dtos.Auth;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Auth.Commands;

public record RegisterCommand(string PhoneNumber, string Password, UserRole Role)
    : IRequest<Result<UserDto>>;