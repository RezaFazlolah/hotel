using Application.Models;
using Application.Queries.ReservationQueries;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
using MediatR;

namespace Application.Handlers.QueryHandlers.ReservationQueryHandlers;

public class GetAllReservationsHandler(
    IReservationService reservationService,
    IUserService userService,
    IGuestService guestService,
    IManagerService managerService,
    IAdminService adminService)
    : IRequestHandler<GetAllReservationsQuery, Result<ICollection<Reservation>>>
{
    public async Task<Result<ICollection<Reservation>>> Handle(GetAllReservationsQuery request,
        CancellationToken ct)
    {
        var roles = await userService.GetRolesAsync(request.UserId, ct);
        ICollection<Reservation> reservations;

        if (roles.Contains(UserRole.Admin))
            reservations = await adminService.GetReservationsAsync(request.UserId, ct);
        else if (roles.Contains(UserRole.Manager))
            reservations = await managerService.GetReservationsAsync(request.UserId, ct);
        else if (roles.Contains(UserRole.Guest))
            reservations = await guestService.GetReservationsAsync(request.UserId, ct);
        else
            return Result<ICollection<Reservation>>.Failure(new Error("user role is not supported"), 400);

        return Result<ICollection<Reservation>>.Success(reservations);
    }
}