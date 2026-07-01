using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paging;

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

    Task<Result<PagedResult<Reservation>>> GetAllByHotelIdAsync(
        Guid hotelId,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<Reservation>>> GetAllByHotelIdsAsync(
        IEnumerable<Guid> hotelIds,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<Reservation>>> GetAllByRoomIdAsync(
        Guid roomId,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<Reservation>>> GetAllByRoomIdsAsync(
        IEnumerable<Guid> roomIds,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<Reservation>>> GetAllByGuestIdAsync(
        Guid guestId,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<Reservation>>> GetAllByManagerIdAsync(
        Guid managerId,
        PaginationParameters paginationParameters,
        CancellationToken ct);
}