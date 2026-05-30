using Application.Interfaces.Repositories;
using Application.Requests.RoomRequests;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.RoomHandlers;

public class UpdateRoomHandler(IRoomRepository roomRepository, IHotelRepository hotelRepository, IMapper mapper)
    : IRequestHandler<UpdateRoom, Result<Room>>
{
    public async Task<Result<Room>> Handle(UpdateRoom request, CancellationToken cancellationToken)
    {
        var errors = new List<Error>();
        var roomResult = await roomRepository.GetByIdAsync(request.Id, cancellationToken);
        if (!roomResult.Succeeded)
            errors.Add(new Error($"room {request.Id} not found"));
        if (request.HotelId != null &&
            await hotelRepository.GetByIdAsync(request.HotelId.Value, cancellationToken) == null)
            errors.Add(new Error($"hotel {request.HotelId} not found"));
        if (errors.Count > 0)
            return Result<Room>.Failure(errors, ResultCode.NotFound);
        var room = roomResult.Value;
        mapper.Map(request, room);
        return await roomRepository.UpdateAsync(room, cancellationToken);
    }
}