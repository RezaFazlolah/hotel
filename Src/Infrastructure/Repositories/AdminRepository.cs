using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Infrastructure.Repositories;

public class AdminRepository(
    AppDbContext db,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
    : UserRepository(db, userManager),
        IAdminRepository
{
}