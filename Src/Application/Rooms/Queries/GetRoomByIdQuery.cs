using MediatR;
using SharedKernel.Common;

namespace Application.Rooms.Queries;

public class GetRoomByIdQuery
    : IRequest<Result<Domain.Models.Room>>
{
    public required Guid RoomId { get; set; }
}