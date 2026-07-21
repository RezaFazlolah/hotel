using Application.Hotels.Dtos;
using Application.Hotels.Queries;
using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Hotels.Handlers;

public class GetHotelByIdQueryHandler(
    IHotelQueryService hotelQueryService,
    ICurrentUserService currentUserService,
    IManagerRepository managerRepository)
    : IRequestHandler<GetHotelByIdQuery, Result<HotelDto>>
{
    public async Task<Result<HotelDto>> Handle(GetHotelByIdQuery request, CancellationToken ct)
    {
        var rootError = new Error("get hotel by id failed");

        var currentUserInfoResult = await currentUserService.GetCurrentUserInfoAsync(ct);
        if (!currentUserInfoResult.Succeeded)
            return Result<HotelDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            return await hotelQueryService.GetByIdAsync(request.HotelId, ct);
        }

        if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            var hotelIdResult = await managerRepository.GetHotelIdAsync(currentUserInfo.id, ct);
            if (!hotelIdResult.Succeeded)
                return Result<HotelDto>.Failure(hotelIdResult.Errors.Prepend(rootError));
            var hotelId = hotelIdResult.Value;

            if (request.HotelId == hotelId)
                return await hotelQueryService.GetByIdAsync(request.HotelId, ct);
            return Result<HotelDto>.Failure([rootError, new Error($"hotel {request.HotelId} not found")]);
        }

        if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            return await hotelQueryService.GetByIdAsync(request.HotelId, ct);
        }

        return Result<HotelDto>.Failure(
            [rootError, new Error("forbidden request", ErrorCode.Forbidden)], ResultCode.Forbidden);
    }
}