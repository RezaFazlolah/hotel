using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.ServiceInterfaces;

public interface IManagerService
    : IUserService
{
    Task<Result<Guid>> GetHotelIdAsync(Guid managerId, CancellationToken ct);
    Task<Result<ICollection<Guid>>> GetHotelsIdAsync(IEnumerable<Guid> managersId, CancellationToken ct);
}