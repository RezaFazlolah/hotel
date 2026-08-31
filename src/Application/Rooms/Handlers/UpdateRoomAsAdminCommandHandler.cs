using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Rooms.Commands;
using Application.Rooms.Dtos;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Rooms.Handlers;

public class UpdateRoomAsAdminCommandHandler(
    IHotelRepository hotelRepository,
    IRoomRepository roomRepository,
    ICurrentUserService currentUserService,
    IMapper mapper)
    : IRequestHandler<UpdateRoomAsAdminCommand, Result<RoomDto>>
{
    public async Task<Result<RoomDto>> Handle(
        UpdateRoomAsAdminCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"update room {request.Id} failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<RoomDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (!currentUserInfo.roles.Contains(UserRole.Admin))
            return Result<RoomDto>.Forbidden(rootError);

        var roomExists = await roomRepository.ExistsAsync(request.Id, ct);
        if (!roomExists)
            return Result<RoomDto>.Failure([rootError, new Error("room not found", ErrorCode.NotFound)], ResultCode.NotFound);

        var hotelExists = await hotelRepository.ExistsAsync(request.HotelId, ct);
        if (!hotelExists)
            return Result<RoomDto>.Failure([rootError, new Error("hotel not found", ErrorCode.NotFound)],
                ResultCode.NotFound);

        var roomNumberExists = await roomRepository.NumberExistsAsync(request.HotelId, request.Number, ct);
        if (roomNumberExists)
            return Result<RoomDto>.Failure([rootError, new Error($"room number {request.Number} already exists")]);

        var updatedRoom = mapper.Map<Room>(request);
        var result = await roomRepository.UpdateAsync(updatedRoom, ct);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return Result<RoomDto>.Handle(resultDto, rootError);
    }
}