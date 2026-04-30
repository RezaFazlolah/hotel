using Application.Models;
using Application.Queries.ReservationQueries;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.QueryHandlers.ReservationQueryHandlers;

public class GetAllReservationsHandler(IReservationRepository reservationRepository)
    : IRequestHandler<GetAllReservationsQuery, Result<ICollection<Reservation>>>
{
    public async Task<Result<ICollection<Reservation>>> Handle(GetAllReservationsQuery request,
        CancellationToken cancellationToken)
    {
        var guestIdString = request.GuestId.ToString();
        var reservations =
            await reservationRepository.GetAllAsync(cancellationToken, filterOn: "GuestId", filterQuery: guestIdString);

        return Result<ICollection<Reservation>>.Success(reservations);
    }
}