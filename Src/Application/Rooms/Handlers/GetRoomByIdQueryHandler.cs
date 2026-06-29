using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Application.Rooms.Dtos;
using Application.Rooms.Queries;
using AutoMapper;
using MediatR;
using SharedKernel.Common;

namespace Application.Rooms.Handlers;

public class GetRoomByIdQueryHandler(IRoomQueryService roomQueryService)
    : IRequestHandler<GetRoomByIdQuery, Result<RoomDto>>
{
    public async Task<Result<RoomDto>> Handle(GetRoomByIdQuery request, CancellationToken ct)
        => await roomQueryService.GetByIdAsync(request.RoomId, ct);
}