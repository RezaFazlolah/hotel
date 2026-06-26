using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IRoomRepository
    : IBaseRepository<Guid, Room>
{
    Task<Result<ICollection<Reservation>>> GetReservationsAsync(Guid roomId, CancellationToken ct);
    Task<Result<ICollection<Reservation>>> GetReservationsAsync(IEnumerable<Guid> roomsId, CancellationToken ct);
}