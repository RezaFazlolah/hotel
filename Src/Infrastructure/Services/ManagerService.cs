using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel.Common;

namespace Infrastructure.Services;

public class ManagerService(
    IHotelService hotelService,
    IRoomService roomService,
    IReservationService reservationService,
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IConfiguration configuration)
    : UserService(context, userManager, roleManager), IManagerService
{
    public override async Task<Result<ICollection<Reservation>>> GetReservationsAsync(Guid managerId, CancellationToken ct)
        // implement with ReservationService's GetReservations() with proper filter instead of this
    {
        throw new NotImplementedException();
        // var hotelId = await GetHotelIdAsync(managerId, ct);
        // return await hotelService.GetReservationsAsync(hotelId, ct);
    }

    public async Task<Result<Guid>> GetHotelIdAsync(Guid managerId, CancellationToken ct)
    {
        var managerResult = await GetByIdAsync(managerId, ct);
        if (!managerResult.Succeeded)
            return Result<Guid>.Failure(managerResult.Errors);
        var manager = (Manager) managerResult.Value;
        var hotelId = manager.HotelId;
        return hotelId == null
            ? Result<Guid>.Failure(new Error($"manager {managerId} doesnt manage any hotel."))
            : Result<Guid>.Success(hotelId.Value);
    }

    public async Task<Result<ICollection<Guid>>> GetHotelsIdAsync(IEnumerable<Guid> managersId, CancellationToken ct)
        => throw new NotImplementedException();
}