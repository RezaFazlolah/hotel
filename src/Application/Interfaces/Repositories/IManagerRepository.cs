using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IManagerRepository
    : IUserRepository
{
    Task<Result<Guid?>> GetHotelIdAsync(
        Guid managerId,
        CancellationToken ct);

    Task<bool> ManagesHotel(
        Guid managerId,
        Guid hotelId,
        CancellationToken ct);

    // IRoomRepository.IsManagedByManagerAsync(Guid roomId, Guid managerId, CancellationToken ct) does the same thing
    Task<bool> ManagesRoomAsync(
        Guid managerId,
        Guid roomId,
        CancellationToken ct);
}