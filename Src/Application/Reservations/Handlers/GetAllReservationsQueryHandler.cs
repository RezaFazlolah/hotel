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
        var rolesResult = currentUserService.Roles;
        if (!rolesResult.Succeeded)
            return Result<PagedResult<ReservationDto>>.Failure(
                rolesResult.Errors.Prepend(new Error("get all reservations failed.")));
        var roles = rolesResult.Value;
        var userId = currentUserService.Id.Value;

        if (roles.Contains(UserRole.Admin))
            return await reservationQueryService.GetAllAsync(request.PaginationParameters, ct);

        if (roles.Contains(UserRole.Manager))
        {
            var reservations = await reservationRepository.GetAllByManagerIdAsync(userId, request.PaginationParameters, ct);
            return mapper.Map<Result<PagedResult<ReservationDto>>>(reservations);
        }

        if (roles.Contains(UserRole.Guest))
        {
            var reservations = await reservationRepository.GetAllByGuestIdAsync(userId, request.PaginationParameters, ct);
            return mapper.Map<Result<PagedResult<ReservationDto>>>(reservations);
        }

        return Result<PagedResult<ReservationDto>>.Failure(new Error("user role not supported."));
    }
}