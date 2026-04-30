using Application.Models;
using Application.Queries.ReservationQueries;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.QueryHandlers.ReservationQueryHandlers;

public class GetReservationByIdHandler(IReservationRepository reservationRepository)
    : IRequestHandler<GetReservationByIdQuery, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetByIdAsync(request.ReservationId, cancellationToken);
        if (reservation == null || reservation.GuestId != request.GuestId)
            return Result<Reservation>.Failure(new Error($"reservation {request.ReservationId} not found"), 404);
        return Result<Reservation>.Success(reservation);
    }
}