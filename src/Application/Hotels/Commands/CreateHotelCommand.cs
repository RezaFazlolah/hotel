using Application.Hotels.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Commands;

public record CreateHotelCommand
    : IRequest<Result<HotelDto>>
{
    public required string Name { get; init; }
    public required string Address { get; init; }
    public float Rating { get; init; }
    public Guid? ManagerId { get; init; }
    public IEnumerable<Guid> RoomIds { get; init; } = [];
}