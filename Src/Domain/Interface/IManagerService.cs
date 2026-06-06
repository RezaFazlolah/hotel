using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Domain.Interface;

public interface IManagerService
    : IUserService
{
    // Result<IEnumerable<Guid>> GetRoomsIdAsync(Guid managerId, CancellationToken ct);
}