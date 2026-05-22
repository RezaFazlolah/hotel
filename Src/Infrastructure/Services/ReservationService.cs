using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Services;

public class ReservationService(AppDbContext context, IRoomService roomService)
    : BaseService<Guid, Reservation>(context), IReservationService
{
    public override async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
        => await context.Reservations.AnyAsync(r => r.Id == id && r.Status != ReservationStatus.Cancelled,
            ct);

    public async Task<Result<decimal>> CalculateTotalPriceAsync(Guid roomId, DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,
        CancellationToken ct)
        => Result<decimal>.Success((int)(checkOutDate - checkInDate).TotalDays *
                                   ((await roomService.GetByIdAsync(roomId, ct)).Value).PricePerNight);

    public async Task<Result<Reservation>> CancelAsync(Guid reservationId, CancellationToken ct)
    {
        var result = await GetByIdAsync(reservationId, ct);
        if(!result.Succeeded)
            return result;
        var reservation = result.Value;
        reservation.Status = ReservationStatus.Cancelled;
        await context.SaveChangesAsync(ct);
        return Result<Reservation>.Success(reservation);
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