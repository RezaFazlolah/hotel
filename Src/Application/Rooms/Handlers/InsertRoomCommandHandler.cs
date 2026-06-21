using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Rooms.Commands;
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
    : IRequestHandler<InsertRoomCommand, Result<Room>>
{
    public async Task<Result<Room>> Handle(InsertRoomCommand request, CancellationToken ct)
    {
        var userRolesResult = currentUserService.Roles;
        if (!userRolesResult.Succeeded)
            return Result<Room>.Failure(userRolesResult.Errors.Prepend(new Error("insert room failed.")));
        var userRoles = userRolesResult.Value;
        var userId = currentUserService.Id.Value;

        if (userRoles.Contains(UserRole.Admin))
        {
            if (!await hotelRepository.ExistsAsync(request.HotelId, ct))
                return Result<Room>.Failure(new Error($"insert room failed. hotel {request.HotelId} not found."));
        }
        else if (userRoles.Contains(UserRole.Manager))
        {
            var hotelIdResult = await managerRepository.GetHotelIdAsync(userId, ct);
            if (!hotelIdResult.Succeeded)
                return Result<Room>.Failure(hotelIdResult.Errors.Prepend(new Error("insert room failed.")));
            var hotelId = hotelIdResult.Value;

            if (request.HotelId != hotelId)
                return Result<Room>.Failure(new Error($"insert room failed. hotel {request.HotelId} not found."));
        }
        else
            return Result<Room>.Failure(new Error("insert room failed. unauthorized access.", ErrorCode.Forbidden),
                ResultCode.Forbidden);

        var roomNumberExistsResult =
            await hotelRepository.RoomNumberExistsAsync(request.Number, request.HotelId, ct);
        if (!roomNumberExistsResult.Succeeded)
            return Result<Room>.Failure(roomNumberExistsResult.Errors.Prepend(new Error($"insert room failed.")));
        var roomNumberExists = roomNumberExistsResult.Value;

        return roomNumberExists
            ? Result<Room>.Failure(new Error(
                $"insert room failed. hotel {request.HotelId} already has room number {request.Number}."))
            : await roomRepository.InsertAsync(mapper.Map<Room>(request), ct);
    }
}