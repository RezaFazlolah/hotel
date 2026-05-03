using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class DbInitializer()
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { UserRoles.Admin, UserRoles.Guest };
        foreach (var roleName in roleNames)
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));

        var adminUser = new User { UserName = "+989184129577", PhoneNumber = "+989184129577" };
        var guestUser = new User { UserName = "+989216073852", PhoneNumber = "+989216073852" };

        var adminExists = userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == adminUser.PhoneNumber).Result;
        if (adminExists == null)
        {
            var result = await userManager.CreateAsync(adminUser, "1234");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        var guestExists = userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == guestUser.PhoneNumber).Result;
        if (guestExists == null)
        {
            var result = await userManager.CreateAsync(guestUser, "1234");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(guestUser, "Guest");
        }
    }
}