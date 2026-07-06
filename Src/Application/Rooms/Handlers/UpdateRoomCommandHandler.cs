using Application.Interfaces;
using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Rooms.Commands;
using Application.Rooms.Dtos;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Rooms.Handlers;

public class UpdateRoomCommandHandler(
    IRoomRepository roomRepository,
    ICurrentUserService currentUserService,
    IManagerService managerService,
    IMapper mapper)
    : IRequestHandler<UpdateRoomCommand, Result<RoomDto>>
{
    public async Task<Result<RoomDto>> Handle(UpdateRoomCommand request, CancellationToken ct)
    {
        var callerIdResult = currentUserService.Id;
        if (!callerIdResult.Succeeded)
            return Result<RoomDto>.Failure(callerIdResult.Errors);
        var callerId = callerIdResult.Value;

        var callerRolesResult = currentUserService.Roles;
        if (!callerRolesResult.Succeeded)
            return Result<RoomDto>.Failure(callerRolesResult.Errors);
        var callerRoles = callerRolesResult.Value;

        if (callerRoles.Contains(UserRole.Admin))
        {
            if (!await roomRepository.ExistsAsync(request.Id, ct))
                return Result<RoomDto>.Failure(new Error($"update room {request.Id} failed. room not found."));
        }
        else if (callerRoles.Contains(UserRole.Manager))
        {
            var roomsIdResult = await managerService.GetAllRoomsIdAsync(callerId, ct);
            if (!roomsIdResult.Succeeded)
                return Result<RoomDto>.Failure(roomsIdResult.Errors);
            var roomsId = roomsIdResult.Value;
            if (!roomsId.Contains(request.Id))
                return Result<RoomDto>.Failure(new Error($"update room {request.Id} failed. room not found."));
        }
        else
            return Result<RoomDto>.Failure(new Error($"update room {request.Id} failed. unauthorized access."));

        var updatedRoom = mapper.Map<Room>(request);

        var result = await roomRepository.UpdateAsync(updatedRoom, ct);
        return mapper.Map<Result<RoomDto>>(result);
    }
}