using Application.Interfaces.ServiceInterfaces;
using Application.Queries.ReservationQueries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.QueryHandlers.ReservationQueryHandlers;

public class GetAllReservationsHandler(
    ICurrentUserService currentUserService,
    IUserService userService,
    IGuestService guestService,
    IManagerService managerService,
    IAdminService adminService)
    : IRequestHandler<GetAllReservationsQuery, Result<ICollection<Reservation>>>
{
    public async Task<Result<ICollection<Reservation>>> Handle(GetAllReservationsQuery request,
        CancellationToken ct)
    {
        var userId = currentUserService.Id;
        var roles = (await userService.GetRolesAsync(userId, ct)).Value;

        if (roles.Contains(UserRole.Admin))
            return await adminService.GetReservationsAsync(userId, ct);
        if (roles.Contains(UserRole.Manager))
            return await managerService.GetReservationsAsync(userId, ct);
        if (roles.Contains(UserRole.Guest))
            return await guestService.GetReservationsAsync(userId, ct);
        return Result<ICollection<Reservation>>.Failure(new Error("user role not supported"));
    }
}