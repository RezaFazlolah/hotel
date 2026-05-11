using Domain.Models;

namespace Application.Interfaces.ServiceInterfaces;

public interface IManagerService
    : IUserService
{
    Task<ICollection<Reservation>> GetReservationsAsync(Guid managerId, CancellationToken ct);
    Task<Guid> GetHotelIdAsync(Guid managerId, CancellationToken ct);
    Task<ICollection<Guid>> GetHotelsIdAsync(IEnumerable<Guid> managersId, CancellationToken ct);
}