using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel.Common;

namespace Infrastructure.Repositories;

public class AdminRepository(
    AppDbContext db,
    UserManager<User> userManager)
    : UserRepository(db, userManager),
        IAdminRepository
{
    public override async Task<bool> ExistsAsync(Guid adminId, CancellationToken ct)
        => await db.Admins.AnyAsync(g => g.Id == adminId, ct);
}