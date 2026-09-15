using Application.Interfaces.Repositories;
using Domain.Interfaces;
using Domain.Models;
using SharedKernel.Common;

namespace Application.Reservations.Services;

public class ReservationService(
    IRoomRepository roomRepository)
    : IReservationService
{
    public async Task<Result> CalculatePriceAsync(
        Reservation reservation,
        CancellationToken ct)
    {
        var roomResult = await roomRepository.GetByIdAsync(reservation.RoomId, ct);
        if (!roomResult.Succeeded)
            return Result.Failure(roomResult.Errors);
        var room = roomResult.Value;

        reservation.SetTotalPrice(room.PricePerNight);
        return Result.Success();
    }
}