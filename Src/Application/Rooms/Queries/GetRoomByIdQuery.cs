using Application.Dtos.RoomDtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Rooms.Queries;

public class GetRoomByIdQuery
    : IRequest<Result<RoomDto>>
{
    public required Guid RoomId { get; set; }
}