using Application.Models;
using Application.Queries.ReservationQueries;
using Domain.Models;
using Domain.Services;
using MediatR;

namespace Application.Handlers.QueryHandlers.ReservationQueryHandlers;

public class GetAllReservationsHandler(IReservationService reservationService)
    : IRequestHandler<GetAllReservationsQuery, Result<ICollection<Reservation>>>
{
    public async Task<Result<ICollection<Reservation>>> Handle(GetAllReservationsQuery request,
        CancellationToken cancellationToken)
    {
        var guestIdString = request.GuestId.ToString();
        var reservations =
            await reservationService.GetAllAsync(cancellationToken, filterOn: "GuestId", filterQuery: guestIdString);

        return Result<ICollection<Reservation>>.Success(reservations);
    }
}