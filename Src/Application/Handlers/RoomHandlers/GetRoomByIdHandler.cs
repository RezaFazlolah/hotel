using Application.Interfaces.Repositories;
using Application.Requests.RoomRequests;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.RoomHandlers;

public class GetRoomByIdHandler(IRoomRepository roomRepository) : IRequestHandler<GetRoomById, Result<Room>>
{
    public async Task<Result<Room>> Handle(GetRoomById request, CancellationToken ct)
        => await roomRepository.GetByIdAsync(request.RoomId, ct);
}