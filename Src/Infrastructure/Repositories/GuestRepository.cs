using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class GuestRepository(
    AppDbContext db,
    UserManager<User> userManager)
    : UserRepository(db, userManager),
        IGuestRepository
{
}