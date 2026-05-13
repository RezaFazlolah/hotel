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
    public override async Task<ICollection<Reservation>> GetReservationsAsync(Guid adminId, CancellationToken ct)
        // implement with ReservationService's GetReservations() with proper filter instead of this
        => throw new NotImplementedException();
}