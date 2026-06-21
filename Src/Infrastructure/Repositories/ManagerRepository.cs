using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Infrastructure.Repositories;

public class ManagerRepository(
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
    : UserRepository(context, userManager, roleManager), IManagerRepository
{
    // public override async Task<Result<PagedResult<Reservation>>> GetAllReservationsAsync(Guid managerId,
    //     PaginationParameters paginationParameters, CancellationToken ct)
    // {
    //     // get manager's hotelID
    //     var hotelIdResult = await GetHotelIdAsync(managerId, ct);
    //     if (!hotelIdResult.Succeeded)
    //         return Result<PagedResult<Reservation>>.Failure(hotelIdResult.Errors);
    //     var nullableHotelId = hotelIdResult.Value;
    //     if (nullableHotelId is null)
    //         return Result<PagedResult<Reservation>>.Success(new PagedResult<Reservation>() { Data = [] });
    //     var hotelId = nullableHotelId.Value;
    //
    //     // get hotel's rooms
    //     var roomsIdResult = await hotelRepository.GetRoomsIdAsync(hotelId, ct);
    //     if (!roomsIdResult.Succeeded)
    //         return Result<PagedResult<Reservation>>.Failure(roomsIdResult.Errors);
    //     var roomsId = roomsIdResult.Value;
    //
    //     // compare
    //     var reservations = await reservationRepository.GetAllAsQueryable()
    //         .Where(r => roomsId.Contains(r.RoomId)).ToPagedResultAsync(paginationParameters, ct);
    //     return Result<PagedResult<Reservation>>.Success(reservations);
    // }

    public async Task<Result<Guid?>> GetHotelIdAsync(Guid managerId, CancellationToken ct)
    {
        var managerResult = await GetByIdAsync(managerId, ct);
        if (!managerResult.Succeeded)
            return Result<Guid?>.Failure(managerResult.Errors);
        var manager = (Manager)managerResult.Value;
        var hotelId = manager.HotelId;
        return Result<Guid?>.Success(hotelId);

        // approach 2
        // return (await context.Managers.SingleAsync(m => m.Id == managerId, ct)).HotelId;
    }
}