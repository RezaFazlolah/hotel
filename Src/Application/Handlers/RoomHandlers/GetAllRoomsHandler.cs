using Application.Interfaces.ServiceInterfaces;
using Application.Queries.RoomQueries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Handlers.RoomHandlers;

public class GetAllRoomsHandler(IRoomService roomService)
    : IRequestHandler<GetAllRooms, Result<PagedResult<Room>>>
{
    public async Task<Result<PagedResult<Room>>> Handle(GetAllRooms request,
        CancellationToken ct)
        => await roomService.GetAllAsync(request.PaginationParameters, ct);
}