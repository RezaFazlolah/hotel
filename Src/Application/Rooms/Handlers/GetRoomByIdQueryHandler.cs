using Application.Interfaces.Repositories;
using Application.Rooms.Dtos;
using Application.Rooms.Queries;
using AutoMapper;
using MediatR;
using SharedKernel.Common;

namespace Application.Rooms.Handlers;

public class GetRoomByIdQueryHandler(IRoomRepository roomRepository, IMapper mapper)
    : IRequestHandler<GetRoomByIdQuery, Result<RoomDto>>
{
    public async Task<Result<RoomDto>> Handle(GetRoomByIdQuery request, CancellationToken ct)
    {
        var result = await roomRepository.GetByIdAsync(request.RoomId, ct);
        return mapper.Map<Result<RoomDto>>(result);
    }
}