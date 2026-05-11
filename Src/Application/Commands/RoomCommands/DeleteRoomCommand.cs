using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Commands.RoomCommands;

public class DeleteRoomCommand : IRequest<Result<Room>>
{
    public required Guid RoomId { get; set; }
}