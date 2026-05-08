using Domain.Models;

namespace Domain.Services;

public interface IManagerService
    : IUserService
{
    Task<ICollection<Reservation>> GetReservationsAsync(Guid managerId, CancellationToken ct);
    Task<Guid> GetHotelIdAsync(Guid managerId, CancellationToken ct);
    Task<ICollection<Guid>> GetHotelsIdAsync(IEnumerable<Guid> managersId, CancellationToken ct);
}