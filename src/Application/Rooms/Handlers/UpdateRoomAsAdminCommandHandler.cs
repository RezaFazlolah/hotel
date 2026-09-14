using Application.Common.Extensions;
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
        var rootError = new Error($"update room {request.RoomId} failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<RoomDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (!currentUserInfo.roles.Contains(UserRole.Admin))
            return Result<RoomDto>.Forbidden(rootError);

        var roomResult = await roomRepository.GetByIdAsync(request.RoomId, ct);
        if(!roomResult.Succeeded)
            return Result<RoomDto>.Failure(roomResult.Errors.Prepend(rootError));
        var room =  roomResult.Value;
        
        var newHotelExists = await hotelRepository.ExistsAsync(request.HotelId, ct);
        if (!newHotelExists)
            return Result<RoomDto>.Failure([rootError, new Error($"hotel {request.HotelId} not found", ErrorCode.NotFound)],
                ResultCode.NotFound);

        var roomNumberExists = await roomRepository.NumberExistsAsync(request.HotelId, request.Number, ct);
        if (roomNumberExists)
            return Result<RoomDto>.Failure([rootError, new Error($"room number {request.Number} already exists")]);

        mapper.Map(request, room);
        var result = await roomRepository.UpdateWithReloadAsync(room, ct);
        var resultDto = result.Map<Room, RoomDto>(mapper);
        return Result<RoomDto>.Handle(resultDto, rootError);
    }
}