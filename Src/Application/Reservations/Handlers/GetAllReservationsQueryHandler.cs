using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Reservations.Dtos;
using Application.Reservations.Queries;
using AutoMapper;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Paginations;

namespace Application.Reservations.Handlers;

public class GetAllReservationsQueryHandler(
    ICurrentUserService currentUserService,
    IReservationRepository reservationRepository,
    IReservationQueryService reservationQueryService,
    IMapper mapper)
    : IRequestHandler<GetAllReservationsQuery, Result<PagedResult<ReservationDto>>>
{
    public async Task<Result<PagedResult<ReservationDto>>> Handle(
        GetAllReservationsQuery request,
        CancellationToken ct)
    {
        var rootError = new Error("get all reservations failed");

        var currentUserInfoResult = await currentUserService.GetCurrentUserInfoAsync(ct);
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
            var reservations =
                await reservationRepository.GetAllByManagerIdAsync(currentUserInfo.id, request.PaginationParameters,
                    ct);
            return mapper.Map<Result<PagedResult<ReservationDto>>>(reservations);
        }

        if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            var reservations =
                await reservationRepository.GetAllByGuestIdAsync(currentUserInfo.id, request.PaginationParameters, ct);
            return mapper.Map<Result<PagedResult<ReservationDto>>>(reservations);
        }

        return Result<PagedResult<ReservationDto>>.Failure([
            rootError, new Error("forbidden request", ErrorCode.Forbidden)
        ]);
    }
}