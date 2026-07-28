using Application.Hotels.Dtos;
using Application.Hotels.Queries;
using Application.Interfaces.QueryServices;
using Application.Interfaces.Services;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Hotels.Handlers;

public class GetHotelByIdQueryHandler(
    IHotelQueryService hotelQueryService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetHotelByIdQuery, Result<HotelDto>>
{
    public async Task<Result<HotelDto>> Handle(GetHotelByIdQuery request, CancellationToken ct)
    {
        var rootError = new Error("get hotel by id failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<HotelDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        var hotelDtoResult = await hotelQueryService.GetByIdAsync(request.HotelId, ct);
        if (!hotelDtoResult.Succeeded)
            return Result<HotelDto>.Failure(hotelDtoResult.Errors.Prepend(rootError));
        var hotelDto = hotelDtoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            return hotelDtoResult;
        }

        if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            return currentUserInfo.id == hotelDto.ManagerId
                ? hotelDtoResult
                : Result<HotelDto>.Failure(
                    [rootError, new Error($"hotel {request.HotelId} not found", ErrorCode.NotFound)],
                    ResultCode.NotFound);
        }

        if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            return hotelDtoResult;
        }

        return Result<HotelDto>.Failure([rootError, new Error("forbidden request", ErrorCode.Forbidden)],
            ResultCode.Forbidden);
    }
}