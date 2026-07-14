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
    IPaginator paginator,
    IMapper mapper)
    : IRequestHandler<GetAllHotelsQuery, Result<PagedResult<HotelDto>>>
{
    public async Task<Result<PagedResult<HotelDto>>> Handle(
        GetAllHotelsQuery request,
        CancellationToken ct)
    {
        var rootError = new Error($"get all hotels failed");

        var currentUserInfoResult = await currentUserService.GetCurrentUserInfoAsync(ct);
        if (!currentUserInfoResult.Succeeded)
            return Result<PagedResult<HotelDto>>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            return await hotelQueryService.GetAllAsync(request.PaginationParameters, ct);
        }

        if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            var manager = (Manager)currentUserInfo.user;
            var hotelDto = mapper.Map<HotelDto>(manager.Hotel);

            return hotelDto is null
                ? Result<PagedResult<HotelDto>>.Success(
                    paginator.Paginate<HotelDto>([], request.PaginationParameters, 0))
                : Result<PagedResult<HotelDto>>.Success(
                    paginator.Paginate([hotelDto], request.PaginationParameters, 1));
        }

        if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            return await hotelQueryService.GetAllAsync(request.PaginationParameters, ct);
        }

        return Result<PagedResult<HotelDto>>.Failure([
            rootError, new Error($"forbidden request", ErrorCode.Forbidden)
        ]);
    }
}