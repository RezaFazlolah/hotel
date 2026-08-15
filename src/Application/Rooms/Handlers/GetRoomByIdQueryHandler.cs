using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Rooms.Dtos;
using Application.Rooms.Queries;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Rooms.Handlers;

public class GetRoomByIdQueryHandler(
    IRoomQueryService roomQueryService,
    ICurrentUserService currentUserService,
    IManagerRepository managerRepository)
    : IRequestHandler<GetRoomByIdQuery, Result<RoomDto>>
{
    public async Task<Result<RoomDto>> Handle(
        GetRoomByIdQuery request,
        CancellationToken ct)
    {
        var rootError = new Error($"get room {request.RoomId} failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<RoomDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        Result<RoomDto> result;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            result = await roomQueryService.GetByIdAsync(request.RoomId, ct);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            if (!await managerRepository.ManagesRoomAsync(currentUserInfo.id, request.RoomId, ct))
                return Result<RoomDto>.Failure([rootError, new Error("room not found", ErrorCode.NotFound)],
                    ResultCode.NotFound);

            result = await roomQueryService.GetByIdAsync(request.RoomId, ct);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            result = await roomQueryService.GetByIdAsync(request.RoomId, ct);
        }
        else
        {
            return Result<RoomDto>.Forbidden(rootError);
        }

        return Result<RoomDto>.Handle(result, rootError);
    }
}