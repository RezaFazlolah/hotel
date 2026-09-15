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

        reservation.TotalPrice = CalculatePrice(reservation.CheckInDate, reservation.CheckOutDate, room.PricePerNight);
        return Result.Success();
    }

    public decimal CalculatePrice(
        DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,
        decimal pricePerNight)
        => (checkOutDate.Date - checkInDate.Date).Days * pricePerNight;
}