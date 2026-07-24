using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Rooms.Dtos;
using Application.Rooms.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Rooms.Handlers;

public class GetRoomByIdQueryHandler(
    IRoomQueryService roomQueryService,
    ICurrentUserService currentUserService,
    IManagerService managerService)
    : IRequestHandler<GetRoomByIdQuery, Result<RoomDto>>
{
    public async Task<Result<RoomDto>> Handle(
        GetRoomByIdQuery request,
        CancellationToken ct)
    {
        var rootError = new Error($"get room {request.RoomId} failed");
        var currentUserInfoResult = await currentUserService.GetCurrentUserInfoAsync(ct);
        if (!currentUserInfoResult.Succeeded)
            return Result<RoomDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            return await roomQueryService.GetByIdAsync(request.RoomId, ct);
        }

        if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            return await managerService.ManagesRoomAsync(currentUserInfo.id, request.RoomId, ct)
                ? await roomQueryService.GetByIdAsync(request.RoomId, ct)
                : Result<RoomDto>.Failure([rootError, new Error("room not found", ErrorCode.NotFound)],
                    ResultCode.NotFound);
        }

        if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            return await roomQueryService.GetByIdAsync(request.RoomId, ct);
        }

        return Result<RoomDto>.Failure([rootError, new Error("forbidden request", ErrorCode.Forbidden)],
            ResultCode.Forbidden);
    }
}