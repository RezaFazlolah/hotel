using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ReservationService(AppDbContext context)
    : BaseService<Reservation, Guid>(context), IReservationRepository
{
    public async Task<bool> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate,
        Guid? guestId = null)
    {
        if (guestId == null)
        {
            var isReserved = await context.Reservations.AnyAsync(r =>
                r.RoomId == roomId && !(r.CheckOutDate < checkInDate || checkOutDate < r.CheckInDate));
            return isReserved;
        }
        else
        {
            var isReserved = await context.Reservations.AnyAsync(r =>
                r.RoomId == roomId && !(r.CheckOutDate < checkInDate || checkOutDate < r.CheckInDate) &&
                r.GuestId != guestId);
            return isReserved;
        }
    }

    protected override IQueryable<Reservation> CustomContext()
    {
        return context.Reservations
            .Include(r => r.Room);
        // .Include(r => r.Guest);
    }

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