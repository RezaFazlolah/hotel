using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Repositories;

public class GuestRepository(
    AppDbContext db,
    UserManager<User> userManager,
    IDistributedCache cache)
    : UserRepository(db, userManager, cache),
        IGuestRepository
{
    public override async Task<bool> ExistsAsync(
        Guid guestId,
        CancellationToken ct)
        => await db.Guests
            .AnyAsync(g => g.Id == guestId, ct);
}