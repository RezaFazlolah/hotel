using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Repositories;

public class HotelRepository(AppDbContext db)
    : BaseRepository<Guid, Hotel>(db),
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
}