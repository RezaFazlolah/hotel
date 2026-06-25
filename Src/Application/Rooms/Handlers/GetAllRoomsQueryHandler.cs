using Application.Interfaces.Repositories;
using Application.Rooms.Dtos;
using Application.Rooms.Queries;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Rooms.Handlers;

public class GetAllRoomsQueryHandler(IRoomRepository roomRepository, IMapper mapper)
    : IRequestHandler<GetAllRoomsQuery, Result<PagedResult<RoomDto>>>
{
    public async Task<Result<PagedResult<RoomDto>>> Handle(GetAllRoomsQuery request,
        CancellationToken ct)
    {
        var result = await roomRepository.GetAllAsync(request.PaginationParameters, ct);
        return mapper.Map<Result<PagedResult<RoomDto>>>(result);
    }
}