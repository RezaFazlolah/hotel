using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SharedKernel.Common;

namespace Infrastructure.Repositories;

public class AdminRepository(
    AppDbContext db,
    UserManager<User> userManager)
    : UserRepository(db, userManager),
        IAdminRepository
{
}