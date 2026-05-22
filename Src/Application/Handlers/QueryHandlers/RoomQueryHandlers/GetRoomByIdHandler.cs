using Application.Interfaces.ServiceInterfaces;
using Application.Queries.RoomQueries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.QueryHandlers.RoomQueryHandlers;

public class GetRoomByIdHandler(IRoomService roomService) : IRequestHandler<GetRoomByIdQuery, Result<Room>>
{
    public async Task<Result<Room>> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        => await roomService.GetByIdAsync(request.RoomId, cancellationToken);
}