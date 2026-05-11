using Application.Commands.RoomCommands;
using Application.Interfaces.ServiceInterfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.CommandHandlers.RoomCommandHandlers;

public class UpdateRoomHandler(IRoomService roomService, IHotelService hotelService, IMapper mapper)
    : IRequestHandler<UpdateRoomCommand, Result<Room>>
{
    public async Task<Result<Room>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<Error>();
        var room = await roomService.GetByIdAsync(request.Id, cancellationToken);
        if (room == null)
            errors.Add(new Error($"room {request.Id} not found"));
        if (request.HotelId != null &&
            await hotelService.GetByIdAsync(request.HotelId.Value, cancellationToken) == null)
            errors.Add(new Error($"hotel {request.HotelId} not found"));
        if (errors.Count > 0)
            return Result<Room>.Failure(errors, 404);

        mapper.Map(request, room);
        var updatedRoom = await roomService.UpdateAsync(room, cancellationToken);

        return updatedRoom == null
            ? Result<Room>.Failure(new Error("update room failed"), 400)
            : Result<Room>.Success(updatedRoom);
    }
}