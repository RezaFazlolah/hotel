using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GuestRepository(
    AppDbContext db,
    UserManager<User> userManager)
    : UserRepository(db, userManager),
        IGuestRepository
{
    public override async Task<bool> ExistsAsync(Guid guestId, CancellationToken ct)
        => await db.Guests.AnyAsync(g => g.Id == guestId, ct);
}