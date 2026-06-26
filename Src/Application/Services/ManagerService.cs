using Application.Interfaces.Repositories;
using Domain.Interface;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Extension;
using SharedKernel.Paging;

namespace Application.Services;

public class ManagerService(
    IManagerRepository managerRepository,
    IHotelRepository hotelRepository,
    IRoomRepository roomRepository)
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

    public override async Task<Result<PagedResult<Reservation>>> GetAllReservationsAsync(Guid managerId,
        PaginationParameters paginationParameters, CancellationToken ct)
    {
        var hotelIdResult = await managerRepository.GetHotelIdAsync(managerId, ct);
        if (!hotelIdResult.Succeeded)
            return Result<PagedResult<Reservation>>.Failure(hotelIdResult.Errors);
        var nullableHotelId = hotelIdResult.Value;
        if (nullableHotelId is null)
            return Result<PagedResult<Reservation>>.Success(new PagedResult<Reservation> { Data = [] });
        var hotelId = nullableHotelId.Value;

        var roomsIdResult = await hotelRepository.GetRoomsIdAsync(hotelId, ct);
        if (!roomsIdResult.Succeeded)
            return Result<PagedResult<Reservation>>.Failure(roomsIdResult.Errors);
        var roomsId = roomsIdResult.Value;

        var reservatrionsResult = await roomRepository.GetReservationsAsync(roomsId, ct);
        if (!reservatrionsResult.Succeeded)
            return Result<PagedResult<Reservation>>.Failure(reservatrionsResult.Errors);
        var reservations = reservatrionsResult.Value;

        return Result<PagedResult<Reservation>>.Success(reservations.ToPagedResult(paginationParameters));
    }

    public async Task<Result<IEnumerable<Guid>>> GetAllRoomsIdAsync(Guid managerId, CancellationToken ct)
    {
        var hotelIdResult = await managerRepository.GetHotelIdAsync(managerId, ct);
        if (!hotelIdResult.Succeeded)
            return Result<IEnumerable<Guid>>.Failure(hotelIdResult.Errors);
        var hotelId = hotelIdResult.Value;

        if (hotelId is null)
            return Result<IEnumerable<Guid>>.Success([]);
        
        var roomsIdResult = await hotelRepository.GetRoomsIdAsync(hotelId.Value, ct);
        if (!roomsIdResult.Succeeded)
            return Result<IEnumerable<Guid>>.Failure(roomsIdResult.Errors);
        var roomsId = roomsIdResult.Value;
        
        return Result<IEnumerable<Guid>>.Success(roomsId);
    }

    public async Task<Result<bool>> ManagesRoomsAsync(Guid managerId, Guid roomId, CancellationToken ct)
    {
        var roomsIdResult = await GetAllRoomsIdAsync(managerId, ct);
        if (!roomsIdResult.Succeeded)
            return Result<bool>.Failure(roomsIdResult.Errors);
        var roomsId = roomsIdResult.Value;

        var result = roomsId.Contains(roomId);
        return Result<bool>.Success(result);
    }
}