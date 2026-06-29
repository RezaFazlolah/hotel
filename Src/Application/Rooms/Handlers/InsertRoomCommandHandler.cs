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
        var userRolesResult = currentUserService.Roles;
        if (!userRolesResult.Succeeded)
            return Result<RoomDto>.Failure(userRolesResult.Errors.Prepend(new Error("insert room failed.")));
        var userRoles = userRolesResult.Value;
        var userId = currentUserService.Id.Value;

        if (userRoles.Contains(UserRole.Admin))
        {
            if (!await hotelRepository.ExistsAsync(request.HotelId, ct))
                return Result<RoomDto>.Failure(new Error($"insert room failed. hotel {request.HotelId} not found."));
        }
        else if (userRoles.Contains(UserRole.Manager))
        {
            var hotelIdResult = await managerRepository.GetHotelIdAsync(userId, ct);
            if (!hotelIdResult.Succeeded)
                return Result<RoomDto>.Failure(hotelIdResult.Errors.Prepend(new Error("insert room failed.")));
            var hotelId = hotelIdResult.Value;

            if (request.HotelId != hotelId)
                return Result<RoomDto>.Failure(new Error($"insert room failed. hotel {request.HotelId} not found."));
        }
        else
            return Result<RoomDto>.Failure(new Error("insert room failed. unauthorized access.", ErrorCode.Forbidden),
                ResultCode.Forbidden);

        var roomNumberExistsResult =
            await roomRepository.RoomNumberExistsAsync(request.HotelId, request.Number, ct);
        if (!roomNumberExistsResult.Succeeded)
            return Result<RoomDto>.Failure(roomNumberExistsResult.Errors.Prepend(new Error($"insert room failed.")));
        var roomNumberExists = roomNumberExistsResult.Value;

        if (roomNumberExists)
        {
            return Result<RoomDto>.Failure(new Error(
                $"insert room failed. hotel {request.HotelId} already has room number {request.Number}."));
        }
        else
        {
            var result = await roomRepository.InsertAsync(mapper.Map<Room>(request), ct);
            return mapper.Map<Result<RoomDto>>(result);
        }
    }
}