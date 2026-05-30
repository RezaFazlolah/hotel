using Application.Interfaces.Repositories;
using Application.Requests.ReservationRequests;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.ReservationHandlers;

public class UpdateReservationHandler(
    IReservationRepository reservationRepository,
    ICurrentUserRepository currentUserRepository,
    IRoomRepository roomRepository,
    IMapper mapper)
    : IRequestHandler<UpdateReservation, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(UpdateReservation request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();

        // if (await reservationRepository.ExistsAsync(request.ReservationId, cancellationToken))
        //     return Result<Reservation>.Failure(new Error($"reservation {request.ReservationId} not found"), 404);
        //
        // var reservation = await reservationRepository.GetByIdAsync(request.ReservationId, cancellationToken);
        //
        // var errors = new List<Error>();
        // if (await roomRepository.IsReservedAsync(reservation.RoomId, request.CheckInDate, request.CheckOutDate,
        //         currentUserRepository.Id, cancellationToken))
        //     errors.Add(new Error($"room {reservation.RoomId} is reserved"));
        // if (errors.Count > 0)
        //     return Result<Reservation>.Failure(errors, 404);
        //
        // var r = mapper.Map(request, reservation);
        // reservation.TotalPrice = await reservationRepository.CalculateTotalPriceAsync(reservation.RoomId,
        //     request.CheckInDate, request.CheckOutDate, cancellationToken);
        //
        // var updatedReservation = await reservationRepository.UpdateAsync(reservation, cancellationToken);
        // return updatedReservation == null
        //     ? Result<Reservation>.Failure(new Error("update reservation failed"), 400)
        //     : Result<Reservation>.Success(updatedReservation);
    }
}