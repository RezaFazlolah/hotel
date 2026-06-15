using Application.Interfaces.Repositories;
using Application.Requests.ReservationRequests;
using Domain.Interface;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Paging;

namespace Application.Handlers.ReservationHandlers;

public class GetAllReservationsHandler(
    ICurrentUserService currentUserService,
    IGuestRepository guestRepository,
    IManagerRepository managerRepository,
    IAdminRepository adminRepository,
    IReservationRepository reservationRepository,
    IAdminService adminService,
    IManagerService managerService,
    IGuestService guestService)
    : IRequestHandler<GetAllReservations, Result<PagedResult<Reservation>>>
{
    public async Task<Result<PagedResult<Reservation>>> Handle(GetAllReservations request,
        CancellationToken ct)
    {
        var rolesResult = (await currentUserService.GetRolesAsync(ct));
        if (!rolesResult.Succeeded)
            return Result<PagedResult<Reservation>>.Failure(
                rolesResult.Errors.Prepend(new Error("get all reservations failed.")));
        var roles = rolesResult.Value;
        var userId = currentUserService.Id.Value;

        if (roles.Contains(UserRole.Admin))
            return await adminService.GetAllReservationsAsync(userId, request.PaginationParameters, ct);
        if (roles.Contains(UserRole.Manager))
            return await managerService.GetAllReservationsAsync(userId, request.PaginationParameters, ct);
        if (roles.Contains(UserRole.Guest))
            return await guestService.GetAllReservationsAsync(userId, request.PaginationParameters, ct);

        return Result<PagedResult<Reservation>>.Failure(new Error("user role not supported"));
    }
}