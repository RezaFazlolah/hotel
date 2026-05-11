using Application.Commands.RoomCommands;
using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.CommandHandlers.RoomCommandHandlers;

public class DeleteRoomHandler(IRoomService roomService)
    : IRequestHandler<DeleteRoomCommand, Result<Room>>
{
    public async Task<Result<Room>> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        if (await roomService.GetByIdAsync(request.RoomId, cancellationToken) == null)
            return Result<Room>.Failure(new Error($"room {request.RoomId} not found"), 404);
        var result = await roomService.DeleteAsync(request.RoomId, cancellationToken);

        return result == null
            ? Result<Room>.Failure(new Error($"insert room failed"), 400)
            : Result<Room>.Success(result);
    }
}