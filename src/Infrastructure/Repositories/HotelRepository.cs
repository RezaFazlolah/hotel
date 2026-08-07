using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace Infrastructure.Repositories;

public class HotelRepository(AppDbContext db)
    : BaseRepository<Guid, Hotel>(db),
        IHotelRepository
{
    // public async Task<Result<Guid>> GetIdByManagerIdAsync(
    //     Guid managerId,
    //     CancellationToken ct)
    // {
    //     var hotelResult = await db.Hotels
    //         .SingleOrDefaultAsync(h => h.ManagerId == managerId, ct);
    //
    //     return hotelResult is null
    //         ? Result<Guid>.Failure(new Error($"manager doesn't exist or doesn't manage any hotels"))
    //         : Result<Guid>.Success(hotelResult.Id);
    // }

    // public async Task<Result<Guid?>> GetManagerIdAsync(
    //     Guid hotelId,
    //     CancellationToken ct)
    // {
    //     var result = await db.Hotels.FirstOrDefaultAsync(h => h.Id == hotelId, ct);
    //     return result is null
    //         ? Result<Guid?>.Failure(new Error($"hotel {hotelId} not found"))
    //         : Result<Guid?>.Success(result.ManagerId);
    // }
}