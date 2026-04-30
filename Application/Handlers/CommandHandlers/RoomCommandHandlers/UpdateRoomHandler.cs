using Application.Commands.RoomCommands;
using Application.Models;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.CommandHandlers.RoomCommandHandlers;

public class UpdateRoomHandler(IRoomRepository roomRepository, IHotelRepository hotelRepository, IMapper mapper)
    : IRequestHandler<UpdateRoomCommand, Result<Room>>
{
    public async Task<Result<Room>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<Error>();
        var room = await roomRepository.GetByIdAsync(request.Id, cancellationToken);
        if (room == null)
            errors.Add(new Error($"room {request.Id} not found"));
        if (request.HotelId != null &&
            await hotelRepository.GetByIdAsync(request.HotelId.Value, cancellationToken) == null)
            errors.Add(new Error($"hotel {request.HotelId} not found"));
        if (errors.Count>0)
            return Result<Room>.Failure(errors, 404);

        mapper.Map(request, room);
        var updatedRoom = await roomRepository.UpdateAsync(room, cancellationToken);
        if (updatedRoom == null)
            return Result<Room>.Failure(new Error("update room failed"), 400);
        return Result<Room>.Success(updatedRoom);
    }
}