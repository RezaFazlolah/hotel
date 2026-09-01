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

public class UpdateRoomAsManagerCommandHandler(
    IManagerRepository managerRepository,
    IRoomRepository roomRepository,
    ICurrentUserService currentUserService,
    IMapper mapper)
    : IRequestHandler<UpdateRoomAsManagerCommand, Result<RoomDto>>
{
    public async Task<Result<RoomDto>> Handle(
        UpdateRoomAsManagerCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"update room {request.Id} failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<RoomDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (!currentUserInfo.roles.Contains(UserRole.Manager))
            return Result<RoomDto>.Forbidden(rootError);

        var managesRoom = await managerRepository.ManagesRoomAsync(currentUserInfo.id, request.Id, ct);
        if (!managesRoom)
            return Result<RoomDto>.Failure([rootError, new Error($"room not found", ErrorCode.NotFound)], ResultCode.NotFound);

        var hotelIdResult = await roomRepository.GetHotelIdAsync(request.Id, ct);
        if (!hotelIdResult.Succeeded)
            return Result<RoomDto>.Failure(hotelIdResult.Errors.Prepend(rootError));
        var hotelId = hotelIdResult.Value;

        var roomNumberExists = await roomRepository.NumberExistsAsync(hotelId, request.Number, ct);
        if (roomNumberExists)
            return Result<RoomDto>.Failure([rootError, new Error($"room number {request.Number} already exists")]);

        var updatedRoom = mapper.Map<Room>(request);
        updatedRoom.HotelId = hotelId;
        var result = await roomRepository.UpdateAsync(updatedRoom, ct);
        var resultDto = result.Map<Room, RoomDto>(mapper);
        return Result<RoomDto>.Handle(resultDto, rootError);    }
}