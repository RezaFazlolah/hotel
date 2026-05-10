using System.Numerics;
using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class AdminService(
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IConfiguration configuration)
    : UserService(context, userManager, roleManager), IAdminService
{
    public async Task<ICollection<Reservation>> GetReservationsAsync(Guid adminId, CancellationToken ct)
        => await context.Reservations.ToListAsync(ct);
    // second approach
    // reservationService.GetReservations()
    // which approach is better? both performance-wise and maintenance-wise
}