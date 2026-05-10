using Application.Models;
using Application.Queries.RoomQueries;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.Handlers.QueryHandlers.RoomQueryHandlers;

public class GetRoomByIdHandler(IRoomService roomService) : IRequestHandler<GetRoomByIdQuery, Result<Room>>
{
    public async Task<Result<Room>> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await roomService.GetByIdAsync(request.RoomId, cancellationToken);

        return room == null
            ? Result<Room>.Failure(new Error($"room {request.RoomId} not found"), code: 404)
            : Result<Room>.Success(room);
    }
}