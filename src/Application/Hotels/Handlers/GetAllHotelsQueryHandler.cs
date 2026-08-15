using Application.Hotels.Dtos;
using Application.Hotels.Queries;
using Application.Interfaces.QueryServices;
using Application.Interfaces.Services;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Paginations;

namespace Application.Hotels.Handlers;

public class GetAllHotelsQueryHandler(
    IHotelQueryService hotelQueryService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetAllHotelsQuery, Result<PagedResult<HotelDto>>>
{
    public async Task<Result<PagedResult<HotelDto>>> Handle(
        GetAllHotelsQuery request,
        CancellationToken ct)
    {
        var rootError = new Error($"get all hotels failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<PagedResult<HotelDto>>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        Result<PagedResult<HotelDto>> result;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            result = await hotelQueryService.GetAllAsync(request.HotelFilterParameters, request.HotelSortParameters,
                request.PaginationParameters, ct);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            result = await hotelQueryService.GetAllByManagerAsync(currentUserInfo.id, request.HotelFilterParameters,
                request.HotelSortParameters, request.PaginationParameters, ct);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            result = await hotelQueryService.GetAllAsync(request.HotelFilterParameters, request.HotelSortParameters,
                request.PaginationParameters, ct);
        }
        else
        {
            return Result<PagedResult<HotelDto>>.Forbidden(rootError);
        }

        return Result<PagedResult<HotelDto>>.Handle(result, rootError);
    }
}