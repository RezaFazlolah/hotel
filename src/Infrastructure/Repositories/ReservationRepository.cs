using Application.Interfaces.Repositories;
using Application.Reservations.Filters;
using Application.Reservations.Sorts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Paginations;

namespace Infrastructure.Repositories;

public class ReservationRepository(
    AppDbContext db,
    IRoomRepository roomRepository)
    : BaseRepository<Guid, Reservation>(db),
        IReservationRepository
{
    public override async Task<bool> ExistsAsync(
        Guid id,
        CancellationToken ct)
        => await db.Reservations.AnyAsync(r => r.Id == id && r.Status != ReservationStatus.Cancelled,
            ct);

    // reservation is canceled, not deleted
    public override Task<Result<Reservation>> DeleteAsync(Guid id, CancellationToken ct)
        => throw new NotSupportedException();

    public async Task<bool> IsRoomReservedAsync(
        Guid roomId,
        DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,
        CancellationToken ct)
        => await db.Reservations
            .AnyAsync(r =>
                    r.RoomId == roomId &&
                    r.Status != ReservationStatus.Cancelled &&
                    !(r.CheckOutDate < checkInDate || checkOutDate < r.CheckInDate),
                ct);

    public async Task<bool> IsRoomReservedAsync(
        Guid roomId,
        Guid guestId,
        DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,
        CancellationToken ct)
        => await db.Reservations.AnyAsync(r =>
                r.RoomId == roomId
                && !(r.CheckOutDate < checkInDate || checkOutDate < r.CheckInDate)
                && r.Status != ReservationStatus.Cancelled
                && r.GuestId != guestId,
            ct);

    public async Task<Result<PagedResult<Reservation>>> GetAllByHotelIdAsync(
        Guid hotelId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var roomIdsResult = await roomRepository.GetAllIdsByHotelIdAsync(hotelId, ct);
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

    public async Task<Result<PagedResult<Reservation>>> GetAllByHotelIdsAsync(
        IEnumerable<Guid> hotelIds,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var hotelsIdAsList = hotelIds.ToList();

        var roomIdsResult = await roomRepository.GetAllIdsByHotelIdsAsync(hotelsIdAsList, ct);
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

    public async Task<Result<PagedResult<Reservation>>> GetAllByRoomIdAsync(
        Guid roomId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => await GetAllByRoomIdsAsync([roomId], paginationParameters, ct);

    public async Task<Result<PagedResult<Reservation>>> GetAllByRoomIdsAsync(
        IEnumerable<Guid> roomsId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<Reservation>>.Success(
            await db.Reservations
                .Where(r => roomsId.Contains(r.RoomId))
                .PaginateAsync(paginationParameters, ct)
        );

    public async Task<Result<PagedResult<Reservation>>> GetAllByGuestIdAsync(
        Guid guestId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<Reservation>>.Success(
            await db.Reservations
                .Where(r => r.GuestId == guestId)
                .PaginateAsync(paginationParameters, ct)
        );
}