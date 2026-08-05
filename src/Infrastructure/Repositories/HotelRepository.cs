using Application.Hotels.Filters;
using Application.Hotels.Sorts;
using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace Infrastructure.Repositories;

public class HotelRepository(AppDbContext db)
    : BaseRepository<Guid, Hotel>(db),
        IHotelRepository
{
    public async Task<Result<Guid>> GetIdByManagerIdAsync(
        Guid managerId,
        CancellationToken ct)
    {
        var hotelResult = await db.Hotels
            .SingleOrDefaultAsync(h => h.ManagerId == managerId, ct);

        return hotelResult is null
            ? Result<Guid>.Failure(new Error($"manager ${managerId} doesn't manage any hotel"))
            : Result<Guid>.Success(hotelResult.Id);
    }
}