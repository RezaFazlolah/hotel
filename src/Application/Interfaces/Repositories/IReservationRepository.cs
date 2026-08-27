using Application.Reservations.Filters;
using Application.Reservations.Sorts;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.Repositories;

public interface IReservationRepository
    : IRepositoryBase<Guid, Reservation>
{
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

    Task<Result<PagedResult<Reservation>>> GetAllByHotelAsync(
        Guid hotelId,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<Reservation>>> GetAllByHotelsAsync(
        IEnumerable<Guid> hotelIds,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<Reservation>>> GetAllByRoomAsync(
        Guid roomId,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<Reservation>>> GetAllByRoomsAsync(
        IEnumerable<Guid> roomIds,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<Reservation>>> GetAllByGuestAsync(
        Guid guestId,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    // same as IManagerRepository.ManagesReservationAsync(Guid managerId, Guid reservationId, CancellationToken ct)
    Task<bool> IsManagedByManager(
        Guid reservationId,
        Guid managerId,
        CancellationToken ct);
}