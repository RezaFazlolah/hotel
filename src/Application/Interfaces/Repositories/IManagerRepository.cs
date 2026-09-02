using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IManagerRepository
    : IUserRepository
{
    Task<Result<Manager?>> GetByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct);
    
    Task<Result<Guid?>> GetHotelIdAsync(
        Guid managerId,
        CancellationToken ct);

    Task<bool> ManagesHotelAsync(
        Guid managerId,
        Guid hotelId,
        CancellationToken ct);

    // same as IRoomRepository.IsManagedByManagerAsync(Guid roomId, Guid managerId, CancellationToken ct)
    Task<bool> ManagesRoomAsync(
        Guid managerId,
        Guid roomId,
        CancellationToken ct);
    
    // same as IRoomRepository.IsManagedByManagerAsync(Guid roomId, Guid managerId, CancellationToken ct)
    Task<bool> ManagesReservationAsync(
        Guid managerId,
        Guid reservationId,
        CancellationToken ct);
}