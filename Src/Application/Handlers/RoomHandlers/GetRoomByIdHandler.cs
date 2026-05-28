using Application.Interfaces.ServiceInterfaces;
using Application.Queries.RoomQueries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.RoomHandlers;

public class GetRoomByIdHandler(IRoomService roomService) : IRequestHandler<GetRoomById, Result<Room>>
{
    public async Task<Result<Room>> Handle(GetRoomById request, CancellationToken cancellationToken)
        => await roomService.GetByIdAsync(request.RoomId, cancellationToken);
}