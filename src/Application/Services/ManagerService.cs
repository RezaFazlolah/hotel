using Application.Interfaces.Repositories;
using Domain.Interfaces;

namespace Application.Services;

public class ManagerService(
    IRoomRepository roomRepository,
    IManagerRepository managerRepository)
    : UserService,
        IManagerService
{
    // Question: i don't know if this is the right place to implement this method or not?
    public async Task<bool> ManagesRoomAsync(
        Guid managerId,
        Guid roomId,
        CancellationToken ct)
    {
        var managerHotelIdResult = await managerRepository.GetHotelIdAsync(managerId, ct); 
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