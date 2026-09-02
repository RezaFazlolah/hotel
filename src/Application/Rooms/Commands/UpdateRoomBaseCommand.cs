using Application.Rooms.Dtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Rooms.Commands;

public record UpdateRoomBaseCommand
    : IRequest<Result<RoomDto>>
{
    public required Guid Id { get; init; }
    public required int Number { get; init; }
    public required RoomType Type { get; init; }
    public required decimal PricePerNight { get; init; }
}