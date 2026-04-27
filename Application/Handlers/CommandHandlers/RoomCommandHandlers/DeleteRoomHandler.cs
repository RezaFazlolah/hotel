using Application.Commands.RoomCommands;
using Application.Result;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.CommandHandlers.RoomCommandHandlers;

public class DeleteRoomHandler(IRoomRepository roomRepository)
    : IRequestHandler<DeleteRoomCommand, Result<Room>>
{
    public async Task<Result<Room>> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        if (await roomRepository.GetByIdAsync(request.RoomId, cancellationToken) == null)
            return Result<Room>.Failure($"room {request.RoomId} not found", 404);
        var result = await roomRepository.DeleteAsync(request.RoomId, cancellationToken);
        if (result == null)
            return Result<Room>.Failure($"insert room failed", 400);
        return Result<Room>.Success(result);
    }
}