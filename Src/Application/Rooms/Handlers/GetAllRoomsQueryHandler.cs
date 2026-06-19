using Application.Interfaces.Repositories;
using Application.Rooms.Queries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Rooms.Handlers;

public class GetAllRoomsQueryHandler(IRoomRepository roomRepository)
    : IRequestHandler<GetAllRoomsQuery, Result<PagedResult<Room>>>
{
    public async Task<Result<PagedResult<Room>>> Handle(GetAllRoomsQuery request,
        CancellationToken ct)
        => await roomRepository.GetAllAsync(request.PaginationParameters, ct);
}