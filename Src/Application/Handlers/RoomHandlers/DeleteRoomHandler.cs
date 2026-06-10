using Application.Interfaces.Repositories;
using Application.Requests.RoomRequests;
using Application.Service;
using Domain.Interface;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.RoomHandlers;

public class DeleteRoomHandler(
    IRoomRepository roomRepository,
    IManagerService managerService,
    ICurrentUserRepository currentUserRepository)
    : IRequestHandler<DeleteRoom, Result<Room>>
{
    public async Task<Result<Room>> Handle(DeleteRoom request, CancellationToken ct)
    {
        var callerIdResult = currentUserRepository.Id;
        if (!callerIdResult.Succeeded)
            return Result<Room>.Failure(callerIdResult.Errors);
        var callerId = callerIdResult.Value;

        var callerRolesResult = await currentUserRepository.GetRolesAsync(ct);
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