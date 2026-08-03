using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using SharedKernel.Common;

namespace Application.Services;

public class RoomService(
    IHotelRepository hotelRepository,
    IRoomRepository roomRepository)
    : IRoomService
{
    public async Task<Result<bool>> BelongsToHotelAsync(
        Guid roomId,
        Guid hotelId,
        CancellationToken ct)
    {
        var roomResult = await roomRepository.GetByIdAsync(roomId, ct);
        if (!roomResult.Succeeded)
            return Result<bool>.Failure(roomResult.Errors);
        var room = roomResult.Value;

        var hotelExists = await hotelRepository.ExistsAsync(hotelId, ct);

        return hotelExists
            ? Result<bool>.Success(room.HotelId == hotelId)
            : Result<bool>.Failure(new Error($"hotel {hotelId} not found"));
    }
}