using Application.Commands.RoomCommands;
using Application.Result;
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
        var errorMessage = "";
        var room = await roomRepository.GetByIdAsync(request.Id, cancellationToken);
        if (room == null)
            errorMessage += $"room {request.Id} not found";
        if (request.HotelId != null &&
            await hotelRepository.GetByIdAsync(request.HotelId.Value, cancellationToken) == null)
            errorMessage += $"hotel {request.HotelId} not found";
        if (errorMessage != "")
            return Result<Room>.Failure(errorMessage, 404);

        mapper.Map(request, room);
        var updatedRoom = await roomRepository.UpdateAsync(room, cancellationToken);
        if (updatedRoom == null)
            return Result<Room>.Failure($"update room failed", 400);
        return Result<Room>.Success(updatedRoom);
    }
}