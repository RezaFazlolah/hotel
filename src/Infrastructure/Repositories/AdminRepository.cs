using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AdminRepository(
    AppDbContext db,
    UserManager<User> userManager)
    : UserRepository(db, userManager),
        IAdminRepository
{
    public override async Task<bool> ExistsAsync(
        Guid adminId,
        CancellationToken ct)
        => await db.Admins
            .AnyAsync(a => a.Id == adminId, ct);
}