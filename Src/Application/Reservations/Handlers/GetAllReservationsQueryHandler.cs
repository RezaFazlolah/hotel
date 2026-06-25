using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Reservations.Dtos;
using Application.Reservations.Queries;
using AutoMapper;
using Domain.Interface;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Paging;

namespace Application.Reservations.Handlers;

public class GetAllReservationsQueryHandler(
    ICurrentUserService currentUserService,
    IAdminService adminService,
    IManagerService managerService,
    IGuestService guestService,
    IMapper mapper)
    : IRequestHandler<GetAllReservationsQuery, Result<PagedResult<ReservationDto>>>
{
    public async Task<Result<PagedResult<ReservationDto>>> Handle(GetAllReservationsQuery request,
        CancellationToken ct)
    {
        var rolesResult = currentUserService.Roles;
        if (!rolesResult.Succeeded)
            return Result<PagedResult<ReservationDto>>.Failure(
                rolesResult.Errors.Prepend(new Error("get all reservations failed.")));
        var roles = rolesResult.Value;
        var userId = currentUserService.UserId.Value;

        if (roles.Contains(UserRole.Admin))
        {
            var result = await adminService.GetAllReservationsAsync(userId, request.PaginationParameters, ct);
            return mapper.Map<Result<PagedResult<ReservationDto>>>(result);
        }

        if (roles.Contains(UserRole.Manager))
        {
            var result = await managerService.GetAllReservationsAsync(userId, request.PaginationParameters, ct);
            return mapper.Map<Result<PagedResult<ReservationDto>>>(result);
        }

        if (roles.Contains(UserRole.Guest))
        {
            var result = await guestService.GetAllReservationsAsync(userId, request.PaginationParameters, ct);
            return mapper.Map<Result<PagedResult<ReservationDto>>>(result);
        }

        return Result<PagedResult<ReservationDto>>.Failure(new Error("user role not supported"));
    }
}