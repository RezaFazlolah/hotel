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

        Result<PagedResult<RoomDto>> result;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            result = await roomQueryService.GetAllAsync(request.RoomFilterParameters, request.RoomSortParameters,
                request.PaginationParameters, ct);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            result = await roomQueryService.GetAllByManagerAsync(currentUserInfo.id,
                request.RoomFilterParameters,
                request.RoomSortParameters, request.PaginationParameters, ct);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            result = await roomQueryService.GetAllAsync(request.RoomFilterParameters, request.RoomSortParameters,
                request.PaginationParameters, ct);
        }
        else
        {
            return Result<PagedResult<RoomDto>>.Forbidden(rootError);
        }

        return Result<PagedResult<RoomDto>>.Handle(result, rootError);
    }
}