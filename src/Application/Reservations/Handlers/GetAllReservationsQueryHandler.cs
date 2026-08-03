using Application.Interfaces.QueryServices;
using Application.Interfaces.Services;
using Application.Reservations.Dtos;
using Application.Reservations.Queries;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Paginations;

namespace Application.Reservations.Handlers;

public class GetAllReservationsQueryHandler(
    ICurrentUserService currentUserService,
    IReservationQueryService reservationQueryService)
    : IRequestHandler<GetAllReservationsQuery, Result<PagedResult<ReservationDto>>>
{
    public async Task<Result<PagedResult<ReservationDto>>> Handle(
        GetAllReservationsQuery request,
        CancellationToken ct)
    {
        var rootError = new Error("get all reservations failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<PagedResult<ReservationDto>>.Failure(
                currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            return await reservationQueryService.GetAllAsync(request.PaginationParameters, ct);
        }

        if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            return await reservationQueryService.GetAllByManagerIdAsync(currentUserInfo.id, request.PaginationParameters, ct);
        }

        if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            return await reservationQueryService.GetAllByGuestIdAsync(currentUserInfo.id,
                request.PaginationParameters, ct);
        }

        return Result<PagedResult<ReservationDto>>.Failure([
            rootError, new Error("forbidden request", ErrorCode.Forbidden)
        ]);
    }
}