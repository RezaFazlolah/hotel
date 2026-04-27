using Application.Result;
using Domain.Models;
using MediatR;

namespace Application.Queries.RoomQueries;

public class GetRoomByIdQuery : IRequest<Result<Room>>
{
    public Guid RoomId { get; set; }
}