using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Repositories;

public class ReservationRepository(AppDbContext db)
    : BaseRepository<Guid, Reservation>(db), IReservationRepository
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

    public async Task<bool> IsReservedAsync(
        Guid roomId,
        DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,
        CancellationToken ct)
    {
        var result = await db.Reservations.AnyAsync(r =>
                r.RoomId == roomId &&
                r.Status != ReservationStatus.Cancelled &&
                !(r.CheckOutDate < checkInDate || checkOutDate < r.CheckInDate),
            ct);
        return result;
    }

    public async Task<bool> IsReservedAsync(
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
}