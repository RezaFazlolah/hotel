using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IRoomRepository
    : IBaseRepository<Guid, Room>
{
    Task<Result<ICollection<Reservation>>> GetReservationsAsync(Guid roomId, CancellationToken ct);
    Task<Result<ICollection<Reservation>>> GetReservationsAsync(IEnumerable<Guid> roomsId, CancellationToken ct);

    Task<Result<bool>> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        // check if there is any reservation
        CancellationToken ct);

    Task<Result<bool>> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        // check if there is any reservation, but guestId is ignored, its used for updating reservation
        Guid guestId, CancellationToken ct);
}