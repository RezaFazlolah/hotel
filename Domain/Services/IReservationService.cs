using Domain.Models;

namespace Domain.Services;

public interface IReservationService 
    : IBaseService<Reservation, Guid>
{
    Task<bool> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        Guid? guestId = null);
    ICollection<Reservation> GetByHotel(Guid hotelId);
}