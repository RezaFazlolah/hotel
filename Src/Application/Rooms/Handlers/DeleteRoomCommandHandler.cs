using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Rooms.Commands;
using Domain.Interface;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Rooms.Handlers;

public class DeleteRoomCommandHandler(
    IRoomRepository roomRepository,
    IManagerService managerService,
    ICurrentUserService currentUserService)
    : IRequestHandler<DeleteRoomCommand, Result<Room>>
{
    public async Task<Result<Room>> Handle(DeleteRoomCommand request, CancellationToken ct)
    {
        var callerIdResult = currentUserService.Id;
        if (!callerIdResult.Succeeded)
            return Result<Room>.Failure(callerIdResult.Errors);
        var callerId = callerIdResult.Value;

        var callerRolesResult = currentUserService.Roles;
        if (!callerRolesResult.Succeeded)
            return Result<Room>.Failure(
                callerRolesResult.Errors.Prepend(new Error($"delete room {request.RoomId} failed.")));
        var callerRoles = callerRolesResult.Value;

        if (callerRoles.Contains(UserRole.Admin))
        {
            if (!await roomRepository.ExistsAsync(request.RoomId, ct))
                return Result<Room>.Failure(new Error($"delete room {request.RoomId} failed. room not found."),
                    ResultCode.NotFound);
        }
        else if (callerRoles.Contains(UserRole.Manager))
        {
            var roomsIdResult = await managerService.GetAllRoomsIdAsync(callerId, ct);
            if (!roomsIdResult.Succeeded)
                return Result<Room>.Failure(roomsIdResult.Errors);
            var roomsId = roomsIdResult.Value;
            if (!roomsId.Contains(request.RoomId))
                return Result<Room>.Failure(new Error($"delete room {request.RoomId} failed. room not found."),
                    ResultCode.NotFound);
        }
        else
            return Result<Room>.Failure(new Error($"delete room {request.RoomId} failed. unauthorized access."));

        return await roomRepository.DeleteAsync(request.RoomId, ct);
    }
}