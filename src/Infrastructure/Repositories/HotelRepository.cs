using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Repositories;

public class HotelRepository(
    AppDbContext db,
    IDistributedCache cache)
    : BaseRepository<Guid, Hotel>(db, cache),
        IHotelRepository
{
    public async Task<Result<Guid?>> GetManagerIdAsync(
        Guid hotelId,
        CancellationToken ct)
    {
        var result = await db.Hotels
            .Where(h => h.Id == hotelId)
            .Select(h => new
            {
                ManagerId = h.Manager == null
                    ? (Guid?)null
                    : h.Manager.Id
            })
            .FirstOrDefaultAsync(ct);

        if (result is null)
            return Result<Guid?>.Failure(new Error($"hotel with id {hotelId} not found", ErrorCode.NotFound),
                ResultCode.NotFound);
        return Result<Guid?>.Success(result.ManagerId);
    }

    public async Task<Result<decimal>> GetRatingAsync(
        Guid hotelId,
        CancellationToken ct)
    {
        var ratingResult = await db.Hotels
            .Where(h => h.Id == hotelId)
            .Select(decimal? (h) => h.Rating)
            .FirstOrDefaultAsync(ct);

        return ratingResult is null
            ? Result<decimal>.Failure(new Error($"hotel {hotelId} not found", ErrorCode.NotFound)
                , ResultCode.NotFound)
            : Result<decimal>.Success(ratingResult.Value);
    }
}