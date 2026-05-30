using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IReservationRepository
    : IBaseRepository<Guid, Reservation>
{
    Task<Result<decimal>> CalculateTotalPriceAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        CancellationToken ct);

    Task<Result<Reservation>> CancelAsync(Guid reservationId, CancellationToken ct);
}