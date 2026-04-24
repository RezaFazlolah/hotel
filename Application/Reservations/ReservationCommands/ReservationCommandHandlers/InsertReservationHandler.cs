using Application.Reservations.ReservationCommands.ReservationCommandRequests;
using Application.Reservations.ReservationDtos;
using Application.Result;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Reservations.ReservationCommands.ReservationCommandHandlers;

public class InsertReservationHandler(
    IReservationRepository reservationRepository,
    IRoomRepository roomRepository,
    IMapper mapper)
    : IRequestHandler<InsertReservationCommand, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(InsertReservationCommand request,
        CancellationToken cancellationToken)
    {
        var errorMessage = string.Empty;
        if (await reservationRepository.IsReservedAsync(request.RoomId, request.CheckInDate, request.CheckOutDate))
            errorMessage += $"room {request.RoomId} is already reserved";
        var room = await roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
        if (room == null)
            errorMessage += $"room {request.RoomId} not found";
        if (errorMessage != string.Empty)
            return Result<ReservationDto>.Failure(errorMessage, 400);

        var reservation = mapper.Map<Reservation>(request);
        var days = (decimal)(request.CheckOutDate - request.CheckInDate).TotalDays;
        reservation.TotalPrice = room!.PricePerNight * days;
        var result = await reservationRepository.InsertAsync(reservation, cancellationToken);
        if (result == null)
            return Result<ReservationDto>.Failure("reservation failed", 400);
        var reservationDto = mapper.Map<ReservationDto>(reservation);
        return Result<ReservationDto>.Success(reservationDto);
    }
}