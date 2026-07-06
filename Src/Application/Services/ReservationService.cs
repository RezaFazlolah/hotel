using Application.Interfaces.Repositories;
using Domain.Interfaces;
using Domain.Models;
using SharedKernel.Common;

namespace Application.Services;

public class ReservationService(
    IRoomRepository roomRepository)
    : IReservationService
{
    public async Task<Result<decimal>> CalculatePriceAsync(
        Reservation reservation,
        CancellationToken ct)
    {
        var roomResult = await roomRepository.GetByIdAsync(reservation.RoomId, ct);
        if (!roomResult.Succeeded)
            return Result<decimal>.Failure(
                roomResult.Errors.Prepend(new Error($"calculate reservation {reservation.Id} price failed.")));
        var room = roomResult.Value;

        var reservationTotalPrice =
            (reservation.CheckOutDate.Date - reservation.CheckInDate.Date).Days * room.PricePerNight;
        return Result<decimal>.Success(reservationTotalPrice);
    }
}