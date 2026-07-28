using Application.Interfaces;
using Application.Interfaces.QueryServices;
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

public class InsertRoomCommandHandler(
    IRoomRepository roomRepository,
    IHotelRepository hotelRepository,
    ICurrentUserService currentUserService,
    IManagerRepository managerRepository,
    IMapper mapper)
    : IRequestHandler<InsertRoomCommand, Result<RoomDto>>
{
    public async Task<Result<RoomDto>> Handle(InsertRoomCommand request, CancellationToken ct)
    {
        var rootError = new Error($"insert room failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<RoomDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            if (!await hotelRepository.ExistsAsync(request.HotelId, ct))
                return Result<RoomDto>.Failure(new Error($"insert room failed. hotel {request.HotelId} not found"));
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            var hotelIdResult = await managerRepository.GetHotelIdAsync(currentUserInfo.id, ct);
            if (!hotelIdResult.Succeeded)
                return Result<RoomDto>.Failure(hotelIdResult.Errors.Prepend(new Error("insert room failed")));
            var hotelId = hotelIdResult.Value;

            if (request.HotelId != hotelId)
                return Result<RoomDto>.Failure(new Error($"insert room failed. hotel {request.HotelId} not found"));
        }
        else
            return Result<RoomDto>.Failure(new Error("insert room failed. unauthorized access", ErrorCode.Forbidden),
                ResultCode.Forbidden);

        var roomNumberExistsResult =
            await roomRepository.RoomNumberExistsAsync(request.HotelId, request.Number, ct);
        if (!roomNumberExistsResult.Succeeded)
            return Result<RoomDto>.Failure(roomNumberExistsResult.Errors.Prepend(new Error($"insert room failed")));
        var roomNumberExists = roomNumberExistsResult.Value;

        if (roomNumberExists)
            return Result<RoomDto>.Failure(new Error(
                $"insert room failed. hotel {request.HotelId} already has room number {request.Number}"));

        var result = await roomRepository.InsertAsync(mapper.Map<Room>(request), ct);
        return mapper.Map<Result<RoomDto>>(result);
    }
}