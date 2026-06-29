using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Application.Rooms.Dtos;
using Application.Rooms.Queries;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Rooms.Handlers;

public class GetAllRoomsQueryHandler(IRoomQueryService roomQueryService)
    : IRequestHandler<GetAllRoomsQuery, Result<PagedResult<RoomDto>>>
{
    public async Task<Result<PagedResult<RoomDto>>> Handle(GetAllRoomsQuery request,
        CancellationToken ct)
        => await roomQueryService.GetAllAsync(request.PaginationParameters, ct);
}