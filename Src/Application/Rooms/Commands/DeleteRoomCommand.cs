using Application.Dtos.RoomDtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Rooms.Commands;

public class DeleteRoomCommand
    : IRequest<Result<RoomDto>>
{
    public required Guid RoomId { get; set; }
}