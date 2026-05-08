using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class ManagerService(
    IHotelService hotelService,
    IRoomService roomService,
    IReservationService reservationService,
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IConfiguration configuration)
    : UserService(context, userManager, roleManager, configuration), IManagerService
{
    /// <summary>
    /// after ReservationService's GetReservations() method which supports filtering is properly implemented,
    /// use that instead of querying Reservation table directly from here,
    /// something like reservationService.GetReservations(filterOn= "managerId", filterQuery= managerId)
    /// </summary>
    public async Task<ICollection<Reservation>> GetReservationsAsync(Guid managerId, CancellationToken ct)
    {
        var hotelId = await GetHotelIdAsync(managerId, ct);
        return await hotelService.GetReservationsAsync(hotelId, ct);
    }

    public async Task<Guid> GetHotelIdAsync(Guid managerId, CancellationToken ct)
        => (await context.Managers.FirstAsync(m => m.Id == managerId, ct)).HotelId;

    public async Task<ICollection<Guid>> GetHotelsIdAsync(IEnumerable<Guid> managersId, CancellationToken ct)
        => await context.Managers.Where(m => managersId.Contains(m.HotelId)).Select(m => m.HotelId).ToListAsync(ct);
}