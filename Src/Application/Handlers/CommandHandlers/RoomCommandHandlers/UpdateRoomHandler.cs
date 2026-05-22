using Application.Commands.RoomCommands;
using Application.Interfaces.ServiceInterfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.CommandHandlers.RoomCommandHandlers;

public class UpdateRoomHandler(IRoomService roomService, IHotelService hotelService, IMapper mapper)
    : IRequestHandler<UpdateRoomCommand, Result<Room>>
{
    public async Task<Result<Room>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<Error>();
        var roomResult = await roomService.GetByIdAsync(request.Id, cancellationToken);
        if (!roomResult.Succeeded)
            errors.Add(new Error($"room {request.Id} not found"));
        if (request.HotelId != null &&
            await hotelService.GetByIdAsync(request.HotelId.Value, cancellationToken) == null)
            errors.Add(new Error($"hotel {request.HotelId} not found"));
        if (errors.Count > 0)
            return Result<Room>.Failure(errors, ResultCode.NotFound);
        var room = roomResult.Value;
        mapper.Map(request, room);
        return  await roomService.UpdateAsync(room, cancellationToken);
    }
}