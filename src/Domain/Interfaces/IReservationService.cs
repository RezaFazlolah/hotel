using Domain.Models;
using SharedKernel.Common;

namespace Domain.Interfaces;

public interface IReservationService
{
    public Task<Result<decimal>> CalculatePriceAsync(
        Reservation reservation,
        CancellationToken ct);

    decimal CalculatePrice(
        DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,
        decimal pricePerNight);
}