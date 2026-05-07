using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Queries.RoomQueries;

public class GetRoomByIdQuery : IRequest<Result<Room>>
{
    public required Guid RoomId { get; set; }
}