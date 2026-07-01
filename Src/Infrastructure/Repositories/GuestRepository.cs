using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Common;

namespace Infrastructure.Repositories;

public class GuestRepository(
    AppDbContext db,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
    : UserRepository(db, userManager),
        IGuestRepository
{
}