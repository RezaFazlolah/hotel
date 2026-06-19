using Application.Interfaces.Repositories;
using Application.Rooms.Queries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Rooms.Handlers;

public class GetRoomByIdQueryHandler(IRoomRepository roomRepository)
    : IRequestHandler<GetRoomByIdQuery, Result<Room>>
{
    public async Task<Result<Room>> Handle(GetRoomByIdQuery request, CancellationToken ct)
        => await roomRepository.GetByIdAsync(request.RoomId, ct);
}