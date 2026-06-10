using Application.Interfaces.Repositories;
using Application.Requests.RoomRequests;
using AutoMapper;
using Domain.Interface;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.RoomHandlers;

public class UpdateRoomHandler(
    IRoomRepository roomRepository,
    ICurrentUserRepository currentUserRepository,
    IManagerService managerService,
    IMapper mapper)
    : IRequestHandler<UpdateRoom, Result<Room>>
{
    public async Task<Result<Room>> Handle(UpdateRoom request, CancellationToken ct)
    {
        var callerIdResult = currentUserRepository.Id;
        if (!callerIdResult.Succeeded)
            return Result<Room>.Failure(callerIdResult.Errors);
        var callerId = callerIdResult.Value;

        var callerRolesResult = await currentUserRepository.GetRolesAsync(ct);
        if (!callerRolesResult.Succeeded)
            return Result<Room>.Failure(callerRolesResult.Errors);
        var callerRoles = callerRolesResult.Value;

        if (callerRoles.Contains(UserRole.Admin))
        {
            if (!await roomRepository.ExistsAsync(request.Id, ct))
                return Result<Room>.Failure(new Error($"update room {request.Id} failed. room not found."));
        }
        else if (callerRoles.Contains(UserRole.Manager))
        {
            var roomsIdResult = await managerService.GetAllRoomsIdAsync(callerId, ct);
            if (!roomsIdResult.Succeeded)
                return Result<Room>.Failure(roomsIdResult.Errors);
            var roomsId = roomsIdResult.Value;
            if (!roomsId.Contains(request.Id))
                return Result<Room>.Failure(new Error($"update room {request.Id} failed. room not found."));
        }
        else
            return Result<Room>.Failure(new Error($"update room {request.Id} failed. unauthorized access."));

        var updatedRoom = mapper.Map<Room>(request);

        var result = await roomRepository.UpdateAsync(updatedRoom, ct);
        return result;
    }
}