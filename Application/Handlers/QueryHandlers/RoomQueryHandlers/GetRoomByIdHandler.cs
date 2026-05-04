using Application.Models;
using Application.Queries.RoomQueries;
using Domain.Models;
using Domain.Services;
using MediatR;

namespace Application.Handlers.QueryHandlers.RoomQueryHandlers;

public class GetRoomByIdHandler(IRoomService roomService) : IRequestHandler<GetRoomByIdQuery, Result<Room>>
{
    public async Task<Result<Room>> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await roomService.GetByIdAsync(request.RoomId, cancellationToken);
        if (room == null)
            return Result<Room>.Failure(new Error($"room {request.RoomId} not found"), code: 404);
        return Result<Room>.Success(room);
    }
}