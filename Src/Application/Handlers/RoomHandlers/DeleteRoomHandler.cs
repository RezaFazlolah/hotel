using Application.Interfaces.Repositories;
using Application.Requests.RoomRequests;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.RoomHandlers;

public class DeleteRoomHandler(IRoomRepository roomRepository)
    : IRequestHandler<DeleteRoom, Result<Room>>
{
    public async Task<Result<Room>> Handle(DeleteRoom request, CancellationToken cancellationToken)
    {
        if (await roomRepository.GetByIdAsync(request.RoomId, cancellationToken) == null)
            return Result<Room>.Failure(new Error($"room {request.RoomId} not found"), ResultCode.NotFound);
        return await roomRepository.DeleteAsync(request.RoomId, cancellationToken);
    }
}