using Application.Hotels.HotelDtos;
using Application.Reservations.ReservationCommands.ReservationCommandRequests;
using Application.Reservations.ReservationDtos;
using Application.Result;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Reservations.ReservationCommands.ReservationCommandHandlers;

public class UpdateReservationHandler(IReservationRepository reservationRepository, IMapper mapper)
    : IRequestHandler<UpdateReservationCommand, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        var errorMessage = "";
        var reservation = await reservationRepository.GetByIdAsync(request.ReservationId, cancellationToken);
        if (reservation == null || reservation.GuestId != request.GuestId)
            errorMessage += $"reservation {request.ReservationId} not found";
        if (await reservationRepository.IsReservedAsync(request.RoomId, request.CheckInDate, request.CheckOutDate, request.GuestId))
            errorMessage += $"room {request.RoomId} is already reserved";

        if (errorMessage != "")
            return Result<ReservationDto>.Failure(errorMessage, 404);

        mapper.Map(request, reservation);
        var updatedReservation = await reservationRepository.UpdateAsync(reservation, cancellationToken);
        if (updatedReservation == null)
            return Result<ReservationDto>.Failure("update reservation failed", 400);
        var updatedReservationDto = mapper.Map<ReservationDto>(reservation);
        return Result<ReservationDto>.Success(updatedReservationDto);
    }
}