using Application.Hotels.Dtos;
using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Rooms.Dtos;
using Application.Rooms.Queries;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using SharedKernel.Paginations;

namespace Application.Rooms.Handlers;

public class GetAllRoomsQueryHandler(
    IRoomQueryService roomQueryService,
    IRoomRepository roomRepository,
    ICurrentUserService currentUserService,
    IPaginator paginator,
    IMapper mapper)
    : IRequestHandler<GetAllRoomsQuery, Result<PagedResult<RoomDto>>>
{
    public async Task<Result<PagedResult<RoomDto>>> Handle(
        GetAllRoomsQuery request,
        CancellationToken ct)
    {
        var rootError = new Error($"get all rooms failed");

        var currentUserInfoResult = await currentUserService.GetCurrentUserInfoAsync(ct);
        if (!currentUserInfoResult.Succeeded)
            return Result<PagedResult<RoomDto>>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            return await roomQueryService.GetAllAsync(request.PaginationParameters, ct);
        }

        if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            var manager = (Manager)currentUserInfo.user;

            if (manager.HotelId is null)
                return Result<PagedResult<RoomDto>>.Success(paginator.Paginate<RoomDto>([],
                    request.PaginationParameters, 0));

            return await roomQueryService.GetAllByHotelIdAsync(manager.HotelId.Value, request.PaginationParameters, ct);
        }

        if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            return await roomQueryService.GetAllAsync(request.PaginationParameters, ct);
        }

        return Result<PagedResult<RoomDto>>.Failure([
            rootError, new Error($"forbidden request", ErrorCode.Forbidden)
        ]);
    }
}