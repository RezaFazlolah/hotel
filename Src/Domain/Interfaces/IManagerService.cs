using SharedKernel.Common;

namespace Domain.Interfaces;

public interface IManagerService
    : IUserService
{
    Task<Result<IEnumerable<Guid>>> GetAllRoomsIdAsync(
        Guid managerId,
        CancellationToken ct);

    // Question: i don't know if this is the right place to implement this method or not?
    Task<bool> ManagesRoomAsync(
        Guid managerId,
        Guid roomId,
        CancellationToken ct);
}