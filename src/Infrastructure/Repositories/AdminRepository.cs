using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Repositories;

public class AdminRepository(
    AppDbContext db,
    UserManager<User> userManager,
    IDistributedCache cache)
    : UserRepository(db, userManager, cache),
        IAdminRepository
{
    public override async Task<bool> ExistsAsync(
        Guid adminId,
        CancellationToken ct)
        => await db.Admins
            .AnyAsync(a => a.Id == adminId, ct);
}