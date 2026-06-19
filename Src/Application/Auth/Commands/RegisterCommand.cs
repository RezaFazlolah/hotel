using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Auth.Commands;

public record RegisterCommand(string PhoneNumber, string Password, UserRole Role)
    : IRequest<Result<User>>;