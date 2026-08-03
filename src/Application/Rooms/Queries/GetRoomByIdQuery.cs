using Application.Rooms.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Rooms.Queries;

public record GetRoomByIdQuery(Guid RoomId)
    : IRequest<Result<RoomDto>>;