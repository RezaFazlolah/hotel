using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ReservationService(AppDbContext context, IRoomService roomService)
    : BaseService<Guid, Reservation>(context), IReservationService
{
    public override async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        => await context.Reservations.AnyAsync(r => r.Id == id && r.Status != ReservationStatus.Cancelled,
            cancellationToken);

    public async Task<bool> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        CancellationToken ct)
        => await context.Reservations.AnyAsync(r =>
            r.RoomId == roomId && !(r.CheckOutDate < checkInDate || checkOutDate < r.CheckInDate) &&
            r.Status != ReservationStatus.Cancelled, ct);

    public async Task<bool> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        Guid guestId, CancellationToken ct)
        => await context.Reservations.AnyAsync(r =>
            r.RoomId == roomId && !(r.CheckOutDate < checkInDate ||
                                    checkOutDate < r.CheckInDate && r.Status != ReservationStatus.Cancelled) &&
            r.GuestId != guestId, ct);

    public async Task<decimal> CalculateTotalPriceAsync(Guid roomId, DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,
        CancellationToken ct)
        => (int)(checkOutDate - checkInDate).TotalDays * (await roomService.GetByIdAsync(roomId, ct)).PricePerNight;

    public async Task<Reservation?> CancelAsync(Guid reservationId, CancellationToken ct)
    {
        var reservation = await context.Reservations.SingleAsync(r => r.Id == reservationId, ct);
        reservation.Status = ReservationStatus.Cancelled;
        await context.SaveChangesAsync(ct);
        return reservation;
    }

    protected override IQueryable<Reservation> CustomContext()
        => context.Reservations
            .Include(r => r.Room)
            .Include(r => r.Guest);

    protected override IQueryable<Reservation> CustomFilter(IQueryable<Reservation> query, string? filterOn,
        string? filterQuery)
    {
        if (filterOn.Equals("GuestId", StringComparison.OrdinalIgnoreCase))
            query = query.Where(r => r.GuestId.ToString().Equals(filterQuery));

        return query;
    }

    protected override IQueryable<Reservation> CustomSort(IQueryable<Reservation> query, string? orderBy,
        bool isAscending)
    {
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            // sort by reservation total price
            if (orderBy.Equals("TotalPrice", StringComparison.OrdinalIgnoreCase))
                query = isAscending
                    ? query.OrderBy(r => r.TotalPrice)
                    : query.OrderByDescending(r => r.TotalPrice);
        }

        return query;
    }
}