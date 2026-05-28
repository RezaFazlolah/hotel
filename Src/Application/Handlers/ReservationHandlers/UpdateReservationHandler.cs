using Application.Commands.ReservationCommands;
using Application.Interfaces.ServiceInterfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.ReservationHandlers;

public class UpdateReservationHandler(
    IReservationService reservationService,
    ICurrentUserService currentUserService,
    IRoomService roomService,
    IMapper mapper)
    : IRequestHandler<UpdateReservation, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(UpdateReservation request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        
        // if (await reservationService.ExistsAsync(request.ReservationId, cancellationToken))
        //     return Result<Reservation>.Failure(new Error($"reservation {request.ReservationId} not found"), 404);
        //
        // var reservation = await reservationService.GetByIdAsync(request.ReservationId, cancellationToken);
        //
        // var errors = new List<Error>();
        // if (await roomService.IsReservedAsync(reservation.RoomId, request.CheckInDate, request.CheckOutDate,
        //         currentUserService.Id, cancellationToken))
        //     errors.Add(new Error($"room {reservation.RoomId} is reserved"));
        // if (errors.Count > 0)
        //     return Result<Reservation>.Failure(errors, 404);
        //
        // var r = mapper.Map(request, reservation);
        // reservation.TotalPrice = await reservationService.CalculateTotalPriceAsync(reservation.RoomId,
        //     request.CheckInDate, request.CheckOutDate, cancellationToken);
        //
        // var updatedReservation = await reservationService.UpdateAsync(reservation, cancellationToken);
        // return updatedReservation == null
        //     ? Result<Reservation>.Failure(new Error("update reservation failed"), 400)
        //     : Result<Reservation>.Success(updatedReservation);
    }
}