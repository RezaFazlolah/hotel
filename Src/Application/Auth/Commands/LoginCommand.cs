using Application.Auth.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Auth.Commands;

public record LoginCommand(string PhoneNumber, string Password)
    : IRequest<Result<LoggedinUserDto>>;