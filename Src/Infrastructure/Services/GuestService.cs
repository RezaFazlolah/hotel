using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class GuestService(
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IConfiguration configuration)
    : UserService(context, userManager, roleManager), IGuestService
{
    public async Task<ICollection<Reservation>> GetReservationsAsync(Guid guestId, CancellationToken ct)
        => await context.Reservations.Where(r => r.GuestId == guestId).ToListAsync(ct);
    // second approach
    // after ReservationService's GetReservations() method which supports filtering is properly implemented,
    // reservationService.GetReservations(filterOn="guestId", filterQuery=guestId)
    // which approach is better? both performance-wise and maintenance-wise
}