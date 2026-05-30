using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SharedKernel.Common;
using SharedKernel.Extensions;
using SharedKernel.Paging;

namespace Infrastructure.Repositories;

public class ManagerRepository(
    IHotelRepository hotelRepository,
    IRoomRepository roomRepository,
    IReservationRepository reservationRepository,
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IConfiguration configuration)
    : UserRepository(context, userManager, roleManager), IManagerRepository
{
    public override async Task<Result<PagedResult<Reservation>>> GetAllReservationsAsync(Guid managerId,
        PaginationParameters paginationParameters, CancellationToken ct)
    {
        // get manager's hotelID
        var hotelIdResult = await GetHotelIdAsync(managerId, ct);
        if (!hotelIdResult.Succeeded)
            return Result<PagedResult<Reservation>>.Failure(hotelIdResult.Errors);
        var hotelId = hotelIdResult.Value;
    
        // get hotel's rooms
        var roomsIdResult = await hotelRepository.GetRoomsIdAsync(hotelId, ct);
        if (!roomsIdResult.Succeeded)
            return Result<PagedResult<Reservation>>.Failure(roomsIdResult.Errors);
        var roomsId = roomsIdResult.Value;
    
        // compare
        var reservations = await reservationRepository.GetAllAsQueryable()
            .Where(r => roomsId.Contains(r.RoomId)).ToPagedResultAsync(paginationParameters, ct);
        return Result<PagedResult<Reservation>>.Success(reservations);
    }
    
    public async Task<Result<Guid>> GetHotelIdAsync(Guid managerId, CancellationToken ct)
    {
        var managerResult = await GetByIdAsync(managerId, ct);
        if (!managerResult.Succeeded)
            return Result<Guid>.Failure(managerResult.Errors);
        var manager = (Manager)managerResult.Value;
        var hotelId = manager.HotelId;
        return hotelId == null
            ? Result<Guid>.Failure(new Error($"manager {managerId} doesnt manage any hotel."))
            : Result<Guid>.Success(hotelId.Value);
    }
}