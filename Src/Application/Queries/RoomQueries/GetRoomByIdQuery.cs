using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Queries.RoomQueries;

public class GetRoomByIdQuery : IRequest<Result<Room>>
{
    public required Guid RoomId { get; set; }
}