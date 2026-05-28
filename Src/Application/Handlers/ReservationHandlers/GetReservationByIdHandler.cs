using Application.Interfaces.ServiceInterfaces;
using Application.Queries.ReservationQueries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.ReservationHandlers;

public class GetReservationByIdHandler(IReservationService reservationService)
    : IRequestHandler<GetReservationById, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(GetReservationById request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        // var reservation = await reservationService.GetByIdAsync(request.ReservationId, cancellationToken);
        //
        // return (reservation == null || reservation.GuestId != request.GuestId)
        //     ? Result<Reservation>.Failure(new Error($"reservation {request.ReservationId} not found"), 404)
        //     : Result<Reservation>.Success(reservation);
    }
}