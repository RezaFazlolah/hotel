using Application.Hotels.Filters;
using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace Infrastructure.Repositories;

public class HotelRepository(AppDbContext db)
    : BaseRepository<Guid, Hotel, HotelFilterParameters>(db), IHotelRepository
{
    protected override IQueryable<Hotel> CustomContext()
        => db.Hotels
            .Include(h => h.Rooms)
            .Include(h => h.Manager);

    public async Task<Result<Guid>> GetIdByManagerIdAsync(
        Guid managerId,
        CancellationToken ct)
    {
        var hotelResult = await db.Hotels
            .SingleOrDefaultAsync(h => h.ManagerId == managerId, ct);

        return hotelResult is null
            ? Result<Guid>.Failure(new Error($"manager ${managerId} doesnt manage any hotel"))
            : Result<Guid>.Success(hotelResult.Id);
    }
}