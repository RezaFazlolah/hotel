using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Requests.RoomRequests;

public class DeleteRoom : IRequest<Result<Room>>
{
    public required Guid RoomId { get; set; }
}