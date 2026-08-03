using Application.Hotels.Dtos;
using Application.Hotels.Queries;
using Application.Interfaces.QueryServices;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using SharedKernel.Paginations;

namespace Application.Hotels.Handlers;

public class GetAllHotelsQueryHandler(
    IHotelQueryService hotelQueryService,
    ICurrentUserService currentUserService,
    IPaginator paginator)
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

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            return await hotelQueryService.GetAllAsync(request.PaginationParameters, ct);
        }

        if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            var hotelDtoResult = await hotelQueryService.GetByManagerIdAsync(currentUserInfo.id, ct);
            if (!hotelDtoResult.Succeeded)
                return Result<PagedResult<HotelDto>>.Failure(hotelDtoResult.Errors.Prepend(rootError));
            var hotelDto = hotelDtoResult.Value;

            return Result<PagedResult<HotelDto>>.Success(paginator.Paginate<HotelDto>(
                hotelDto is null ? [] : [hotelDto],
                request.PaginationParameters,
                hotelDto is null ? 0 : 1));
        }

        if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            return await hotelQueryService.GetAllAsync(request.PaginationParameters, ct);
        }

        return Result<PagedResult<HotelDto>>.Failure([rootError, new Error($"forbidden request", ErrorCode.Forbidden)],
            ResultCode.Forbidden);
    }
}