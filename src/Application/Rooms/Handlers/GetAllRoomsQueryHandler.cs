using Application.Interfaces.QueryServices;
using Application.Interfaces.Services;
using Application.Rooms.Dtos;
using Application.Rooms.Queries;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Paginations;

namespace Application.Rooms.Handlers;

public class GetAllRoomsQueryHandler(
    IRoomQueryService roomQueryService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetAllRoomsQuery, Result<PagedResult<RoomDto>>>
{
    public async Task<Result<PagedResult<RoomDto>>> Handle(
        GetAllRoomsQuery request,
        CancellationToken ct)
    {
        var rootError = new Error($"get all rooms failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<PagedResult<RoomDto>>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            await roomQueryService.GetAllAsync(request.RoomFilterParameters, request.PaginationParameters, ct);
        }

        if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            return await roomQueryService.GetAllByManagerIdAsync(currentUserInfo.id, request.PaginationParameters, ct);
        }

        if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            await roomQueryService.GetAllAsync(request.RoomFilterParameters, request.PaginationParameters, ct);
        }

        return Result<PagedResult<RoomDto>>.Failure([
            rootError, new Error($"forbidden request", ErrorCode.Forbidden)
        ]);
    }
}