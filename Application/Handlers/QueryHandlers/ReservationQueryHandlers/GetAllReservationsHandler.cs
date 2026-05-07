using Application.Models;
using Application.Queries.ReservationQueries;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
using MediatR;

namespace Application.Handlers.QueryHandlers.ReservationQueryHandlers;

public class GetAllReservationsHandler(IReservationService reservationService, IUserService userService)
    : IRequestHandler<GetAllReservationsQuery, Result<ICollection<Reservation>>>
{
    public async Task<Result<ICollection<Reservation>>> Handle(GetAllReservationsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.UserId == null)
            return Result<ICollection<Reservation>>.Failure(new Error("user is null"), 400);

        var roles = await userService.GetRolesAsync(request.UserId.Value, cancellationToken);

        if (roles.Contains(UserRole.Admin))
        {
            return Result<ICollection<Reservation>>.Success(await reservationService.GetAllAsync(cancellationToken));
        }
        else if (roles.Contains(UserRole.Manager))
        {
            var hotelId = 
            return Result<ICollection<Reservation>>.Success(reservationService.GetByHotel(hotelId, cancellationToken));
        }
        else if (roles.Contains(UserRole.Guest))
        {
        }
        else
        {
        }

        var reservations =
            await reservationService.GetAllAsync(cancellationToken, filterOn: "GuestId", filterQuery: guestIdString);

        return Result<ICollection<Reservation>>.Success(reservations);
    }
}