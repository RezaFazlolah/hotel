using SharedKernel.Common;

namespace Domain.Interfaces;

public interface IManagerService
    : IUserService
{
    // Question: i don't know if this is the right place to implement this method or not?
    Task<bool> ManagesRoomAsync(
        Guid managerId,
        Guid roomId,
        CancellationToken ct);
}