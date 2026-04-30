using Application.Commands.ReservationCommands;
using Application.Models;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.CommandHandlers.ReservationCommandHandlers;

public class DeleteReservationHandler(IReservationRepository reservationRepository, IMapper mapper)
    : IRequestHandler<DeleteReservationCommand, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetByIdAsync(request.ReservationId, cancellationToken);
        if (reservation == null || reservation.GuestId != request.GuestId)
            return Result<Reservation>.Failure(new Error($"reservation {request.ReservationId} not found"), 404);
        reservation = await reservationRepository.DeleteAsync(reservation.Id, cancellationToken);
        if (reservation == null)
            return Result<Reservation>.Failure(new Error($"delete hotel {request.ReservationId} failed"), 400);
        return Result<Reservation>.Success(reservation);
    }
}