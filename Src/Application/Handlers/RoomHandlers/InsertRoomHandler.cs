using Application.Interfaces.Repositories;
using Application.Requests.RoomRequests;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.RoomHandlers;

public class InsertRoomHandler(IRoomRepository roomRepository, IHotelRepository hotelRepository, IMapper mapper)
    : IRequestHandler<InsertRoom, Result<Room>>
{
    public async Task<Result<Room>> Handle(InsertRoom request, CancellationToken ct)
    {
        if (!await hotelRepository.ExistsAsync(request.HotelId, ct))
            return Result<Room>.Failure(new Error($"insert room failed. hotel {request.HotelId} not found"));

        var roomNumberExistResult = await hotelRepository.RoomNumberExistsAsync(request.Number, request.HotelId, ct);
        if (!roomNumberExistResult.Succeeded)
            Result<Room>.Failure(roomNumberExistResult.Errors);
        if (roomNumberExistResult.Value)
            Result<Room>.Failure(new Error($"insert room failed. room number {request.Number}"), ResultCode.NotFound);


        var room = mapper.Map<Room>(request);
        return await roomRepository.InsertAsync(room, ct);
    }
}