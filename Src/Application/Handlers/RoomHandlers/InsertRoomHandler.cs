using Application.Commands.RoomCommands;
using Application.Interfaces.ServiceInterfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.RoomHandlers;

public class InsertRoomHandler(IRoomService roomService, IHotelService hotelService, IMapper mapper)
    : IRequestHandler<InsertRoom, Result<Room>>
{
    public async Task<Result<Room>> Handle(InsertRoom request, CancellationToken ct)
    {
        if (!await hotelService.ExistsAsync(request.HotelId, ct))
            return Result<Room>.Failure(new Error($"insert room failed. hotel {request.HotelId} not found"));
        
        var roomNumberExistResult = await hotelService.RoomNumberExistsAsync(request.Number, request.HotelId, ct);
        if(!roomNumberExistResult.Succeeded)
            Result<Room>.Failure(roomNumberExistResult.Errors);
        if (roomNumberExistResult.Value)
            Result<Room>.Failure(new Error($"insert room failed. room number {request.Number}"), ResultCode.NotFound);


        var room = mapper.Map<Room>(request);
        return await roomService.InsertAsync(room, ct);
    }
}