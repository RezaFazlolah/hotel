using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Interfaces;
using Domain.Models;
using SharedKernel.Common;

namespace Application.Services;

public class ManagerService(
    IManagerRepository managerRepository,
    IRoomQueryService roomQueryService,
    IRoomRepository roomRepository)
    : UserService,
        IManagerService
{
    public async Task<Result<IEnumerable<Guid>>> GetAllRoomsIdAsync(
        Guid managerId,
        CancellationToken ct)
    {
        var hotelIdResult = await managerRepository.GetHotelIdAsync(managerId, ct);
        if (!hotelIdResult.Succeeded)
            return Result<IEnumerable<Guid>>.Failure(hotelIdResult.Errors);
        var hotelId = hotelIdResult.Value;

        if (hotelId is null)
            return Result<IEnumerable<Guid>>.Success([]);

        var roomIdsResult = await roomQueryService.GetAllIdsByHotelIdAsync(hotelId.Value, ct);
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
        var managerHotelIdResult = await managerRepository.GetHotelIdAsync(managerId, ct);
        if (!managerHotelIdResult.Succeeded)
            return false;
        var managerHotelId = managerHotelIdResult.Value;

        if (managerHotelId is null)
            return false;
        
        var roomHotelIdResult = await roomRepository.GetHotelIdAsync(roomId, ct);
        if(!roomHotelIdResult.Succeeded)
            return false;
        var roomHotelId = roomHotelIdResult.Value;

        return managerHotelId ==  roomHotelId;
    }
}