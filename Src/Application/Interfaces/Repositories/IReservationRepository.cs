using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IReservationRepository
    : IBaseRepository<Guid, Reservation>
{
    Task<Result<Reservation>> CancelAsync(
        Guid reservationId,
        CancellationToken ct);

    Task<bool> IsRoomReservedAsync(
        // check if there is any reservation
        Guid roomId,
        DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,
        CancellationToken ct);

    Task<bool> IsRoomReservedAsync(
        // check if there is any reservation, but guestId is ignored, it's used for updating reservation
        Guid roomId,
        Guid guestId,
        DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,
        CancellationToken ct);

    Task<Result<ICollection<Reservation>>> GetAllByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct);

    Task<Result<ICollection<Reservation>>> GetAllByHotelIdsAsync(
        IEnumerable<Guid> hotelIds,
        CancellationToken ct);
    
    Task<Result<ICollection<Reservation>>> GetAllByRoomIdAsync(
        Guid roomId,
        CancellationToken ct);

    Task<Result<ICollection<Reservation>>> GetAllByRoomIdsAsync(
        IEnumerable<Guid> roomIds,
        CancellationToken ct);
}