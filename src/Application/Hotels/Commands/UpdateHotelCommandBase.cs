using Application.Hotels.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Commands;

public record UpdateHotelCommandBase
    : IRequest<Result<UpdatedHotelDto>>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Address { get; init; }
}