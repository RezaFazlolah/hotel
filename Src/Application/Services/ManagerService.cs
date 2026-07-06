using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Domain.Interfaces;
using Domain.Models;
using SharedKernel.Common;

namespace Application.Services;

public class ManagerService(
    IManagerRepository managerRepository,
    IRoomQueryService roomQueryService)
    : UserService, IManagerService
{
    // public async Task<Result<IEnumerable<Guid>>> GetRoomsIdAsync(Guid managerId, CancellationToken ct)
    // {
    //     var managerResult = await managerRepository.GetByIdAsync(managerId, ct);
    //     if (!managerResult.Succeeded)
    //         return Result<IEnumerable<Guid>>.Failure(new Error($"manager {managerId} not found."));
    //     var manager = (Manager)managerResult.Value;
    //     
    //     if(manager.HotelId is null)
    //         return Result<IEnumerable<Guid>>.Failure(new Error($"manager {managerId} doesn't manage any hotels."));
    //         
    //     var roomsIdResult = await hotelRepository.GetRoomsIdAsync(manager.HotelId.Value, ct);
    //     return roomsIdResult.Succeeded
    //         ? Result<IEnumerable<Guid>>.Success(roomsIdResult.Value)
    //         : Result<IEnumerable<Guid>>.Failure(roomsIdResult.Errors);
    // }

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

    public async Task<Result<bool>> ManagesRoomsAsync(
        Guid managerId,
        Guid roomId,
        CancellationToken ct)
    {
        var roomsIdResult = await GetAllRoomsIdAsync(managerId, ct);
        if (!roomsIdResult.Succeeded)
            return Result<bool>.Failure(roomsIdResult.Errors);
        var roomsId = roomsIdResult.Value;

        var result = roomsId.Contains(roomId);
        return Result<bool>.Success(result);
    }
}