using Application.Interfaces.Repositories;
using Domain.Interfaces;
using SharedKernel.Common;

namespace Application.Services;

public class ManagerService(
    IManagerRepository managerRepository,
    IRoomRepository roomRepository,
    IHotelRepository hotelRepository)
    : UserService,
        IManagerService
{
    public async Task<Result<IEnumerable<Guid>>> GetAllRoomsIdAsync(
        Guid managerId,
        CancellationToken ct)
    {
        var hotelIdResult = await hotelRepository.GetIdByManagerIdAsync(managerId, ct);
        if (!hotelIdResult.Succeeded)
            return Result<IEnumerable<Guid>>.Failure(hotelIdResult.Errors);
        var hotelId = hotelIdResult.Value;

        var roomIdsResult = await roomRepository.GetAllIdsByHotelIdAsync(hotelId, ct);
        if (!roomIdsResult.Succeeded)
            return Result<IEnumerable<Guid>>.Failure(roomIdsResult.Errors);
        var roomIds = roomIdsResult.Value;

        return Result<IEnumerable<Guid>>.Success(roomIds);
    }

    // Question: i don't know if this is the right place to implement this method or not?
    public async Task<bool> ManagesRoomAsync(
        Guid managerId,
        Guid roomId,
        CancellationToken ct)
    {
        var managerHotelIdResult = await hotelRepository.GetIdByManagerIdAsync(managerId, ct); 
        if (!managerHotelIdResult.Succeeded)
            return false;
        var managerHotelId = managerHotelIdResult.Value;
        
        var roomHotelIdResult = await roomRepository.GetHotelIdAsync(roomId, ct);
        if(!roomHotelIdResult.Succeeded)
            return false;
        var roomHotelId = roomHotelIdResult.Value;

        return managerHotelId ==  roomHotelId;
    }
}