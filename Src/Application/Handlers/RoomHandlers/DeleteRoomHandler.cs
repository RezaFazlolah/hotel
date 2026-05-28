using Application.Commands.RoomCommands;
using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.RoomHandlers;

public class DeleteRoomHandler(IRoomService roomService)
    : IRequestHandler<DeleteRoom, Result<Room>>
{
    public async Task<Result<Room>> Handle(DeleteRoom request, CancellationToken cancellationToken)
    {
        if (await roomService.GetByIdAsync(request.RoomId, cancellationToken) == null)
            return Result<Room>.Failure(new Error($"room {request.RoomId} not found"), ResultCode.NotFound);
        return await roomService.DeleteAsync(request.RoomId, cancellationToken);
    }
}