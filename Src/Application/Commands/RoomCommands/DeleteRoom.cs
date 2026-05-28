using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Commands.RoomCommands;

public class DeleteRoom : IRequest<Result<Room>>
{
    public required Guid RoomId { get; set; }
}