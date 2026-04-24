using Domain.Models;

namespace Domain.Repositories;

public interface IReservationRepository : IBaseRepository<Reservation, Guid>
{
    Task<bool> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate, Guid? guestId = null);
}
