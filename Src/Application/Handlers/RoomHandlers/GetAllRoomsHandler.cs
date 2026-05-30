using Application.Interfaces.Repositories;
using Application.Requests.RoomRequests;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Handlers.RoomHandlers;

public class GetAllRoomsHandler(IRoomRepository roomRepository)
    : IRequestHandler<GetAllRooms, Result<PagedResult<Room>>>
{
    public async Task<Result<PagedResult<Room>>> Handle(GetAllRooms request,
        CancellationToken ct)
        => await roomRepository.GetAllAsync(request.PaginationParameters, ct);
}