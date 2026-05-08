using Domain.Models;

namespace Domain.Services;

public interface IReservationService
    : IBaseService<Guid, Reservation>
{
    Task<bool> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        Guid? guestId = null);

    ICollection<Reservation> GetByHotel(Guid hotelId);
}