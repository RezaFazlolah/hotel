using Application.Models;
using Application.Queries.ReservationQueries;
using Domain.Models;
using Domain.Services;
using MediatR;

namespace Application.Handlers.QueryHandlers.ReservationQueryHandlers;

public class GetReservationByIdHandler(IReservationService reservationService)
    : IRequestHandler<GetReservationByIdQuery, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        var reservation = await reservationService.GetByIdAsync(request.ReservationId, cancellationToken);
        if (reservation == null || reservation.GuestId != request.GuestId)
            return Result<Reservation>.Failure(new Error($"reservation {request.ReservationId} not found"), 404);
        return Result<Reservation>.Success(reservation);
    }
}