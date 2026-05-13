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
    public override async Task<ICollection<Reservation>> GetReservationsAsync(Guid guestId, CancellationToken ct)
        // implement with ReservationService's GetReservations() with proper filter instead of this
        => throw new NotImplementedException();
}