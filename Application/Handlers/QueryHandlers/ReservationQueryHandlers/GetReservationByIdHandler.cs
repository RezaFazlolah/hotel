using Application.Models;
using Application.Queries.ReservationQueries;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.Handlers.QueryHandlers.ReservationQueryHandlers;

public class GetReservationByIdHandler(IReservationService reservationService)
    : IRequestHandler<GetReservationByIdQuery, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        // var reservation = await reservationService.GetByIdAsync(request.ReservationId, cancellationToken);
        //
        // return (reservation == null || reservation.GuestId != request.GuestId)
        //     ? Result<Reservation>.Failure(new Error($"reservation {request.ReservationId} not found"), 404)
        //     : Result<Reservation>.Success(reservation);
    }
}