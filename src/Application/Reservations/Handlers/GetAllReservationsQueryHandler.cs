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

        Result<PagedResult<ReservationDto>> result;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            result = await reservationQueryService.GetAllAsync(request.ReservationFilterParameters,
                request.ReservationSortParameters, request.PaginationParameters, ct);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            result = await reservationQueryService.GetAllByManagerAsync(currentUserInfo.id,
                request.ReservationFilterParameters, request.ReservationSortParameters,
                request.PaginationParameters, ct);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            result = await reservationQueryService.GetAllByGuestAsync(currentUserInfo.id,
                request.ReservationFilterParameters, request.ReservationSortParameters,
                request.PaginationParameters, ct);
        }
        else
        {
            return Result<PagedResult<ReservationDto>>.Forbidden(rootError);
        }

        return Result<PagedResult<ReservationDto>>.Handle(result, rootError);
    }
}