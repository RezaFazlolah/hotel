using SharedKernel.Common;

namespace Domain.Interfaces;

public interface IManagerService
    : IUserService
{
    // Result<IEnumerable<Guid>> GetRoomsIdAsync(Guid managerId, CancellationToken ct);

    Task<Result<IEnumerable<Guid>>> GetAllRoomsIdAsync(Guid managerId, CancellationToken ct);
    Task<Result<bool>> ManagesRoomsAsync(Guid managerId, Guid roomId, CancellationToken ct);
}