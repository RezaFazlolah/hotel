using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IRoomRepository
    : IBaseRepository<Guid, Room>
{
    Task<Result<IReadOnlyList<Room>>> GetAllByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct);

    Task<bool> NumberExistsAsync(
        Guid hotelId,
        int roomNumber,
        CancellationToken ct);

    Task<Result<Guid>> GetHotelIdAsync(
        Guid roomId,
        CancellationToken ct);

    Task<Result<Guid?>> GetManagerIdAsync(
        Guid roomId,
        CancellationToken ct);
    
    Task<Result<IReadOnlyList<Guid>>> GetAllIdsByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct);

    Task<Result<IReadOnlyList<Guid>>> GetAllIdsByHotelIdsAsync(
        IEnumerable<Guid> hotelIds,
        CancellationToken ct);

    Task<Result<IReadOnlyList<Guid>>> GetAllIdsByManagerIdAsync(
        Guid managerId,
        CancellationToken ct);
    
    // IManagerRepository.ManagesRoomAsync(Guid managerId, Guid roomId, CancellationToken ct) does the same thing
    Task<bool> IsManagedByManagerAsync(
        Guid roomId,
        Guid managerId,
        CancellationToken ct);

    Task<bool> BelongsToHotelAsync(
        Guid roomId,
        Guid hotelId,
        CancellationToken ct);
}