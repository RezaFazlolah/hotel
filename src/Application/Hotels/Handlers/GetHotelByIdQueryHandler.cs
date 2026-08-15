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
    IManagerRepository managerRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetHotelByIdQuery, Result<HotelDto>>
{
    public async Task<Result<HotelDto>> Handle(
        GetHotelByIdQuery request,
        CancellationToken ct)
    {
        var rootError = new Error($"get hotel {request.HotelId} failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<HotelDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        Result<HotelDto> result;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            result = await hotelQueryService.GetByIdAsync(request.HotelId, ct);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            if (!await managerRepository.ManagesHotel(currentUserInfo.id, request.HotelId, ct))
                return Result<HotelDto>.Failure([rootError, new Error("hotel not found", ErrorCode.NotFound)],
                    ResultCode.NotFound);

            result = await hotelQueryService.GetByIdAsync(request.HotelId, ct);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            result = await hotelQueryService.GetByIdAsync(request.HotelId, ct);
        }
        else
        {
            return Result<HotelDto>.Failure([rootError, new Error("forbidden request", ErrorCode.Forbidden)],
                ResultCode.Forbidden);
        }

        return Result<HotelDto>.Handle(result, rootError, ResultCode.NotFound);
    }
}