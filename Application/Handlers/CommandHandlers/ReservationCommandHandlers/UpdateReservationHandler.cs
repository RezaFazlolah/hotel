using Application.Commands.ReservationCommands;
using Application.Result;
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
        var errorMessage = "";
        var reservation = await reservationRepository.GetByIdAsync(request.ReservationId, cancellationToken);
        if (reservation == null || reservation.GuestId != request.GuestId)
            errorMessage += $"reservation {request.ReservationId} not found";
        if (await reservationRepository.IsReservedAsync(request.RoomId, request.CheckInDate, request.CheckOutDate, request.GuestId))
            errorMessage += $"room {request.RoomId} is already reserved";

        if (errorMessage != "")
            return Result<Reservation>.Failure(errorMessage, 404);

        mapper.Map(request, reservation);
        var updatedReservation = await reservationRepository.UpdateAsync(reservation, cancellationToken);
        if (updatedReservation == null)
            return Result<Reservation>.Failure("update reservation failed", 400);
        return Result<Reservation>.Success(updatedReservation);
    }
}