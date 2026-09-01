using Application.Auth.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Auth.Queries;

public record MeQuery()
    : IRequest<Result<UserDto>>
{
}