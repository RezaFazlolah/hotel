using Application.Interfaces.Repositories;
using Application.Reservations.Queries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Reservations.Handlers;

public class GetReservationByIdQueryHandler(IReservationRepository reservationRepository)
    : IRequestHandler<GetReservationByIdQuery, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        // var reservation = await reservationRepository.GetByIdAsync(request.Id, cancellationToken);
        //
        // return (reservation == null || reservation.GuestId != request.GuestId)
        //     ? Result<Reservation>.Failure(new Error($"reservation {request.Id} not found"), 404)
        //     : Result<Reservation>.Success(reservation);
    }
}