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
        var rootError = new Error("get all reservations failed.");

        var currentUserInfoResult = await currentUserService.GetCurrentUserInfoAsync(ct);
        if (!currentUserInfoResult.Succeeded)
            return Result<PagedResult<ReservationDto>>.Failure(
                currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        var roles = currentUserInfo.roles;
        var userId = currentUserInfo.id;

        if (roles.Contains(UserRole.Admin))
            return await reservationQueryService.GetAllAsync(request.PaginationParameters, ct);
        if (roles.Contains(UserRole.Manager))
        {
            var reservations =
                await reservationRepository.GetAllByManagerIdAsync(userId, request.PaginationParameters, ct);
            return mapper.Map<Result<PagedResult<ReservationDto>>>(reservations);
        }
        if (roles.Contains(UserRole.Guest))
        {
            var reservations =
                await reservationRepository.GetAllByGuestIdAsync(userId, request.PaginationParameters, ct);
            return mapper.Map<Result<PagedResult<ReservationDto>>>(reservations);
        }

        return Result<PagedResult<ReservationDto>>.Failure([
            rootError, new Error("forbidden request", ErrorCode.Forbidden)
        ]);
    }
}