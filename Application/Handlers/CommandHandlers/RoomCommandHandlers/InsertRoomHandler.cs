using Application.Commands.RoomCommands;
using Application.Models;
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
        var errors = new List<Error>();
        if (request.HotelId != null &&
            !await roomRepository.IsRoomNumberUniqueAsync(request.Id, request.HotelId.Value, request.Number, cancellationToken))
            errors.Add(new Error($"room {request.Number} already exists"));
        if (request.HotelId != null &&
            await hotelRepository.GetByIdAsync(request.HotelId.Value, cancellationToken) == null)
            errors.Add(new Error($"hotel {request.HotelId} not found"));
        if (errors.Count > 0)
            return Result<Room>.Failure(errors, 404);

        var room = mapper.Map<Room>(request);
        var result = await roomRepository.InsertAsync(room, cancellationToken);
        if (result == null)
            return Result<Room>.Failure(new Error($"insert room failed"), 400);
        return Result<Room>.Success(result);
    }
}