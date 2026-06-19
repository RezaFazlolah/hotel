using MediatR;
using SharedKernel.Common;

namespace Application.Rooms.Commands;

public class DeleteRoomCommand
    : IRequest<Result<Domain.Models.Room>>
{
    public required Guid RoomId { get; set; }
}