using Application.Commands.ReservationCommands;
using Application.Models;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.CommandHandlers.ReservationCommandHandlers;

public class UpdateReservationHandler(IReservationRepository reservationRepository, IMapper mapper)
    : IRequestHandler<UpdateReservationCommand, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<Error>();
        var reservation = await reservationRepository.GetByIdAsync(request.ReservationId, cancellationToken);
        if (reservation == null || reservation.GuestId != request.GuestId)
            errors.Add(new Error($"reservation {request.ReservationId} not found"));
        if (await reservationRepository.IsReservedAsync(request.RoomId, request.CheckInDate, request.CheckOutDate, request.GuestId))
            errors.Add(new Error($"room {request.RoomId} is already reserved"));

        if (errors.Count > 0)
            return Result<Reservation>.Failure(errors, 404);

        mapper.Map(request, reservation);
        var updatedReservation = await reservationRepository.UpdateAsync(reservation, cancellationToken);
        if (updatedReservation == null)
            return Result<Reservation>.Failure(new Error("update reservation failed"), 400);
        return Result<Reservation>.Success(updatedReservation);
    }
}