using Application.Extensions;
using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Paginations;

namespace Infrastructure.Repositories;

public class ReservationRepository(
    AppDbContext db,
    IRoomQueryService roomQueryService)
    : BaseRepository<Guid, Reservation>(db),
        IReservationRepository
{
    public override async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
        => await db.Reservations.AnyAsync(r => r.Id == id && r.Status != ReservationStatus.Cancelled,
            ct);

    public async Task<Result<Reservation>> CancelAsync(Guid reservationId, CancellationToken ct)
    {
        var result = await GetByIdAsync(reservationId, ct);
        if (!result.Succeeded)
            return result;
        var reservation = result.Value;

        reservation.Status = ReservationStatus.Cancelled;
        await db.SaveChangesAsync(ct);
        return Result<Reservation>.Success(reservation);
    }

    protected override IQueryable<Reservation> CustomContext()
        => db.Reservations
            .Include(r => r.Room)
            .Include(r => r.Guest);

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
                (r.RoomId == roomId &&
                 !(r.CheckOutDate < checkInDate ||
                   checkOutDate < r.CheckInDate && r.Status != ReservationStatus.Cancelled) &&
                 r.GuestId != guestId),
            ct);

    public async Task<Result<PagedResult<Reservation>>> GetAllByHotelIdAsync(
        Guid hotelId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var roomIdsResult = await roomQueryService.GetAllIdsByHotelIdAsync(hotelId, ct);
        if (!roomIdsResult.Succeeded)
            return Result<PagedResult<Reservation>>.Failure(
                roomIdsResult.Errors
                    .Prepend(new Error($"get reservations for hotel {hotelId} failed."))
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

        var roomIdsResult = await roomQueryService.GetAllIdsByHotelIdsAsync(hotelsIdAsList, ct);
        if (!roomIdsResult.Succeeded)
            return Result<PagedResult<Reservation>>.Failure(
                roomIdsResult.Errors
                    .Prepend(new Error($"get reservations for hotels {string.Join(", ", hotelsIdAsList)} failed."))
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

    public async Task<Result<PagedResult<Reservation>>> GetAllByManagerIdAsync(
        Guid managerId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<Reservation>>.Success(
            await db.Reservations
                .Where(r => r.Room.Hotel.Managers.Select(m => m.Id).Contains(managerId))
                .PaginateAsync(paginationParameters, ct)
        );
}