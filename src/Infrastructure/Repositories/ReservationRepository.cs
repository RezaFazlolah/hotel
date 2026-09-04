using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Paginations;

namespace Infrastructure.Repositories;

public class ReservationRepository(
    AppDbContext db,
    IRoomRepository roomRepository,
    IDistributedCache cache)
    : BaseRepository<Guid, Reservation>(db, cache),
        IReservationRepository
{
    public override async Task<bool> ExistsAsync(
        Guid id,
        CancellationToken ct)
        => await db.Reservations
            .AnyAsync(r => r.Id == id
                           && r.Status != ReservationStatus.Cancelled, ct);

    // reservation is cancelled, not deleted
    public override Task<Result<Reservation>> DeleteAsync(Guid id, CancellationToken ct)
        => throw new NotSupportedException();

    public async Task<bool> IsRoomReservedAsync(
        Guid roomId,
        DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,
        CancellationToken ct)
        => await db.Reservations
            .AnyAsync(r =>
                    r.RoomId == roomId
                    && r.Status != ReservationStatus.Cancelled
                    && !(r.CheckOutDate < checkInDate || checkOutDate < r.CheckInDate),
                ct);

    public async Task<bool> IsRoomReservedAsync(
        Guid roomId,
        Guid reservationId,
        DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,
        CancellationToken ct)
        => await db.Reservations.AnyAsync(r =>
                r.RoomId == roomId
                && r.Status != ReservationStatus.Cancelled
                && r.Id != reservationId
                && !(r.CheckOutDate < checkInDate || checkOutDate < r.CheckInDate),
            ct);

    public async Task<Result<PagedResult<Reservation>>> GetAllByHotelAsync(
        Guid hotelId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var roomIdsResult = await roomRepository.GetAllIdsByHotelAsync(hotelId, ct);
        if (!roomIdsResult.Succeeded)
            return Result<PagedResult<Reservation>>.Failure(
                roomIdsResult.Errors
                    .Prepend(new Error($"get reservations for hotel {hotelId} failed"))
            );
        var roomIds = roomIdsResult.Value;

        return Result<PagedResult<Reservation>>.Success(
            await db.Reservations
                .Where(r => roomIds.Contains(r.Id))
                .PaginateAsync(paginationParameters, ct)
        );
    }

    public async Task<Result<PagedResult<Reservation>>> GetAllByHotelsAsync(
        IEnumerable<Guid> hotelIds,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var hotelsIdAsList = hotelIds.ToList();

        var roomIdsResult = await roomRepository.GetAllIdsByHotelsAsync(hotelsIdAsList, ct);
        if (!roomIdsResult.Succeeded)
            return Result<PagedResult<Reservation>>.Failure(
                roomIdsResult.Errors
                    .Prepend(new Error($"get reservations for hotels {string.Join(", ", hotelsIdAsList)} failed"))
            );
        var roomIds = roomIdsResult.Value;

        return Result<PagedResult<Reservation>>.Success(
            await db.Reservations
                .Where(r => roomIds.Contains(r.Id))
                .PaginateAsync(paginationParameters, ct)
        );
    }

    public async Task<Result<PagedResult<Reservation>>> GetAllByRoomAsync(
        Guid roomId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => await GetAllByRoomsAsync([roomId], paginationParameters, ct);

    public async Task<Result<PagedResult<Reservation>>> GetAllByRoomsAsync(
        IEnumerable<Guid> roomsId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<Reservation>>.Success(
            await db.Reservations
                .Where(r => roomsId.Contains(r.RoomId))
                .PaginateAsync(paginationParameters, ct)
        );

    public async Task<Result<PagedResult<Reservation>>> GetAllByGuestAsync(
        Guid guestId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<Reservation>>.Success(
            await db.Reservations
                .Where(r => r.GuestId == guestId)
                .PaginateAsync(paginationParameters, ct)
        );

    // same as ManagerRepository.ManagesReservationAsync(Guid managerId, Guid reservationId, CancellationToken ct)
    public async Task<bool> IsManagedByManager(
        Guid reservationId,
        Guid managerId,
        CancellationToken ct)
        => await db.Reservations
            .AnyAsync(r =>
                r.Id == reservationId
                && r.Room.Hotel.Manager != null
                && r.Room.Hotel.Manager.Id == managerId, ct);

    public async Task<bool> IsReservedByGuest(
        Guid reservationId,
        Guid guestId,
        CancellationToken ct)
        => await db.Reservations.AnyAsync(r =>
            r.Id == reservationId
            && r.GuestId == guestId, ct);
}