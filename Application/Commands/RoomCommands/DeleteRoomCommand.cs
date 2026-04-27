using Application.Result;
using Domain.Models;
using MediatR;

namespace Application.Commands.RoomCommands;

public class DeleteRoomCommand : IRequest<Result<Room>>
{
    public Guid RoomId { get; set; }
}