using Application.Commands.RoomCommands;
using Application.Models;
using AutoMapper;
using Domain.Models;
using Domain.Services;
using MediatR;

namespace Application.Handlers.CommandHandlers.RoomCommandHandlers;

public class InsertRoomHandler(IRoomService roomService, IHotelService hotelService, IMapper mapper)
    : IRequestHandler<InsertRoomCommand, Result<Room>>
{
    public async Task<Result<Room>> Handle(InsertRoomCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<Error>();
        if (!await roomService.IsRoomNumberUniqueAsync(request.Id, request.HotelId, request.Number, cancellationToken))
            errors.Add(new Error($"room {request.Number} already exists"));
        if (await hotelService.GetByIdAsync(request.HotelId, cancellationToken) == null)
            errors.Add(new Error($"hotel {request.HotelId} not found"));
        if (errors.Count > 0)
            return Result<Room>.Failure(errors, 404);

        var room = mapper.Map<Room>(request);
        var result = await roomService.InsertAsync(room, cancellationToken);

        return result == null
            ? Result<Room>.Failure(new Error($"insert room failed"), 400)
            : Result<Room>.Success(result, 201);
    }
}