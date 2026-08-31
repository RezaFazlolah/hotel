using Application.Rooms.Dtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Rooms.Commands;

public record UpdateRoomAsAdminCommand
    : UpdateRoomCommandBase,
        IRequest<Result<RoomDto>>
{
    public required Guid HotelId { get; init; }
}