using Application.Commands.RoomCommands;
using Application.Result;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.CommandHandlers.RoomCommandHandlers;

public class InsertRoomHandler(IRoomRepository roomRepository, IHotelRepository hotelRepository, IMapper mapper)
    : IRequestHandler<InsertRoomCommand, Result<Room>>
{
    public async Task<Result<Room>> Handle(InsertRoomCommand request, CancellationToken cancellationToken)
    {
        var errorMessage = "";
        if (request.HotelId != null &&
            !await roomRepository.IsRoomNumberUniqueAsync(request.Id, request.HotelId.Value, request.Number, cancellationToken))
            errorMessage += $"room {request.Number} already exists";
        if (request.HotelId != null &&
            await hotelRepository.GetByIdAsync(request.HotelId.Value, cancellationToken) == null)
            errorMessage += $"hotel {request.HotelId} not found";
        if (errorMessage != "")
            return Result<Room>.Failure(errorMessage, 404);

        var room = mapper.Map<Room>(request);
        var result = await roomRepository.InsertAsync(room, cancellationToken);
        if (result == null)
            return Result<Room>.Failure($"insert room failed", 400);
        return Result<Room>.Success(result);
    }
}