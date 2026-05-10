using Domain.Models;
using Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ReservationService(AppDbContext context, IRoomService roomService)
    : BaseService<Guid, Reservation>(context), IReservationService
{
    public async Task<bool> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        CancellationToken ct)
        => await context.Reservations.AnyAsync(r =>
            r.RoomId == roomId && !(r.CheckOutDate < checkInDate || checkOutDate < r.CheckInDate), ct);

    public async Task<bool> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        Guid guestId, CancellationToken ct)
        => await context.Reservations.AnyAsync(r =>
            r.RoomId == roomId && !(r.CheckOutDate < checkInDate || checkOutDate < r.CheckInDate) &&
            r.GuestId != guestId, ct);

    public async Task<decimal> CalculateTotalPriceAsync(Guid roomId, DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,
        CancellationToken ct)
        => (int)(checkOutDate - checkInDate).TotalDays * (await roomService.GetByIdAsync(roomId, ct)).PricePerNight;

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