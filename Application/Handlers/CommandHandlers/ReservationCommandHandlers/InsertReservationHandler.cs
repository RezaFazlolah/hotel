using Application.Commands.ReservationCommands;
using Application.Models;
using AutoMapper;
using Domain.Models;
using Domain.Services;
using MediatR;

namespace Application.Handlers.CommandHandlers.ReservationCommandHandlers;

public class InsertReservationHandler(
    IReservationService reservationService,
    IRoomService roomService,
    IMapper mapper)
    : IRequestHandler<InsertReservationCommand, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(InsertReservationCommand request,
        CancellationToken cancellationToken)
    {
        var errors = new List<Error>();
        if (await reservationService.IsReservedAsync(request.RoomId, request.CheckInDate, request.CheckOutDate))
            errors.Add(new Error($"room {request.RoomId} is already reserved"));
        var room = await roomService.GetByIdAsync(request.RoomId, cancellationToken);
        if (room == null)
            errors.Add(new Error($"room {request.RoomId} not found"));
        if (errors.Count > 0)
            return Result<Reservation>.Failure(errors, 400);

        var reservation = mapper.Map<Reservation>(request);
        var days = (decimal)(request.CheckOutDate - request.CheckInDate).TotalDays;
        reservation.TotalPrice = room.PricePerNight * days;
        var result = await reservationService.InsertAsync(reservation, cancellationToken);
        if (result == null)
            return Result<Reservation>.Failure(new Error("reservation failed"), 400);
        return Result<Reservation>.Success(reservation, 201);
    }
}