using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Enums;

namespace Infrastructure;

public class DbInitializer()
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var context = serviceProvider.GetRequiredService<AppDbContext>();

        // hotel
        var hotels = new List<Hotel>
        {
            new() { Id = Guid.Parse("26d4ed41-66a0-4448-a031-84248db7b35c") ,Name = "Parsian", Address = "Hamadan", Rating = 3.8m },
            new() { Id = Guid.Parse("34a4ed41-66a0-4448-a031-84248db7b72f"), Name = "Spinas", Address = "Tehran", Rating = 4.5m }
        };
        
        if(!context.Hotels.Any())
        {
            await context.Hotels.AddRangeAsync(hotels);
            await context.SaveChangesAsync();
        }

        // room
        var rooms = new List<Room>
        {
            new Room { Number = 101, Type = RoomType.Normal, PricePerNight = 100, HotelId = hotels[0].Id },
        };

        if (!context.Rooms.Any())
        {
            await context.Rooms.AddRangeAsync(rooms);
            await context.SaveChangesAsync();
        }

        // roles
        foreach (var role in Enum.GetNames<UserRole>())
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new Role(role));

        // users
        var users = new List<(User user, string role, string password)>
        {
            (new Guest { FirstName = "ali", UserName = "+989184129511", PhoneNumber = "+989184129511" },
                UserRole.Guest.ToString(),
                "1234"),
            (new Manager { FirstName = "hasan", UserName = "+989184129522", PhoneNumber = "+989184129522", HotelId = hotels[0].Id },
                UserRole.Manager.ToString(),
                "1234"),
            (new Admin { FirstName = "mamad", UserName = "+989184129533", PhoneNumber = "+989184129533" },
                UserRole.Admin.ToString(), "1234")
        };

        foreach (var item in users)
        {
            if (await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == item.user.PhoneNumber) == null)
            {
                var result = await userManager.CreateAsync(item.user, item.password);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(item.user, item.role);
            }
        }
    }
}