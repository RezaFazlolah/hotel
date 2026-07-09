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

    // Question: i don't know if this is the right place to implement this method or not?
    public async Task<Result<bool>> ManagesRoomAsync(
        Guid managerId,
        Guid roomId,
        CancellationToken ct)
    {
        var errors = new List<Error>();

        var roomResult = await roomRepository.GetByIdAsync(roomId, ct);
        if (!roomResult.Succeeded)
            errors.AddRange(roomResult.Errors);
        var room = roomResult.Value;

        var managerResult = await managerRepository.GetByIdAsync(managerId, ct);
        if (!managerResult.Succeeded)
            errors.AddRange(managerResult.Errors);
        var manager = (Manager)managerResult.Value;

        return errors.Any()
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(room.HotelId == manager.HotelId);
    }
}