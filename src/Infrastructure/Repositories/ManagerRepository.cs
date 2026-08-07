using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ManagerRepository(
    AppDbContext db,
    UserManager<User> userManager)
    : UserRepository(db, userManager),
        IManagerRepository
{
    public override async Task<bool> ExistsAsync(Guid managerId, CancellationToken ct)
        => await db.Managers.AnyAsync(g => g.Id == managerId, ct);
}