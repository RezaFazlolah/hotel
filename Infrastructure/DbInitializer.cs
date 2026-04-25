using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // 1. Ensure roles exist
        string[] roleNames = { "Admin", "Guest" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // 2. Define users to seed
        var adminUser = new AppUser
        {
            PhoneNumber = "+989184129577",
        };

        var guestUser = new AppUser
        {
            PhoneNumber = "+989216073852",
        };

        if (await userManager.)
        {
            var result = await userManager.CreateAsync(adminUser, "1234"); // adjust password
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
                // Handle errors (in development you may throw or log)
                throw new Exception($"Failed to seed admin user: {string.Join(", ", result.Errors)}");
            }
        }

        if (await userManager.FindByEmailAsync(guestUser.Email) == null)
        {
            var result = await userManager.CreateAsync(guestUser, "Guest@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(guestUser, "Guest");
            }
            else
            {
                throw new Exception($"Failed to seed guest user: {string.Join(", ", result.Errors)}");
            }
        }
    }
}
