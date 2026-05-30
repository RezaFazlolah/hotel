using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Repositories;

public class ReservationRepository(AppDbContext context, IRoomRepository roomRepository)
    : BaseRepository<Guid, Reservation>(context), IReservationRepository
{
    public override async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
        => await context.Reservations.AnyAsync(r => r.Id == id && r.Status != ReservationStatus.Cancelled,
            ct);

    public async Task<Result<decimal>> CalculateTotalPriceAsync(Guid roomId, DateTimeOffset checkInDate,
        DateTimeOffset checkOutDate,
        CancellationToken ct)
        => Result<decimal>.Success((int)(checkOutDate - checkInDate).TotalDays *
                                   ((await roomRepository.GetByIdAsync(roomId, ct)).Value).PricePerNight);

    public async Task<Result<Reservation>> CancelAsync(Guid reservationId, CancellationToken ct)
    {
        var result = await GetByIdAsync(reservationId, ct);
        if (!result.Succeeded)
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
}