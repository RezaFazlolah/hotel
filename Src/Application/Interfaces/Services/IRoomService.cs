using SharedKernel.Common;

namespace Application.Interfaces.Services;

public interface IRoomService
{
    Task<Result<bool>> BelongsToHotelAsync(
        Guid roomId,
        Guid hotelId,
        CancellationToken ct);
}