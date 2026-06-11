using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Constants;
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
            new()
            {
                Id = Guid.Parse("26d4ed41-66a0-4448-a031-84248db7b35c"), Name = "Parsian", Address = "Hamadan",
                Rating = 3.8m
            },
            new()
            {
                Id = Guid.Parse("34a4ed41-66a0-4448-a031-84248db7b72f"), Name = "Spinas", Address = "Tehran",
                Rating = 4.5m
            },
            new()
            {
                Id = Guid.Parse("22593d93-73e2-485a-a680-ba272d8d6b8c"), Name = "Khatam", Address = "Isfahan",
                Rating = 4.9m
            },
        };

        if (!context.Hotels.Any())
        {
            await context.Hotels.AddRangeAsync(hotels);
            await context.SaveChangesAsync();
        }

        // room
        var rooms = new List<Room>
        {
            new Room { Number = 101, Type = RoomType.Normal, PricePerNight = 100, HotelId = hotels[0].Id },
            new Room { Number = 302, Type = RoomType.Vip, PricePerNight = 500, HotelId = hotels[0].Id },
            new Room { Number = 101, Type = RoomType.Vip, PricePerNight = 200, HotelId = hotels[1].Id },
            new Room { Number = 711, Type = RoomType.Normal, PricePerNight = 400, HotelId = hotels[2].Id },
            new Room { Number = 712, Type = RoomType.Normal, PricePerNight = 400, HotelId = hotels[2].Id },
            new Room { Number = 713, Type = RoomType.Vip, PricePerNight = 700, HotelId = hotels[2].Id },
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
           (new Guest { FirstName = "guest1", UserName = "09184129511", PhoneNumber = "09184129511" },
                UserRoleName.Guest, "1234"),
           (new Guest { FirstName = "guest2", UserName = "09184129512", PhoneNumber = "09184129512" },
               UserRoleName.Guest, "1234"),
            (new Manager { FirstName = "manager1", UserName = "09184129521", PhoneNumber = "09184129521", HotelId = hotels[0].Id },
                UserRoleName.Manager, "1234"),
            (new Manager { FirstName = "manager2", UserName = "09184129522", PhoneNumber = "09184129522", HotelId = hotels[2].Id },
                UserRoleName.Manager, "1234"),
            (new Manager { FirstName = "manager3", UserName = "09184129523", PhoneNumber = "09184129523", HotelId = hotels[2].Id },
                UserRoleName.Manager, "1234"),
            (new Admin { FirstName = "admin1", UserName = "09184129531", PhoneNumber = "09184129531" },
                UserRoleName.Admin, "1234"),
            (new Admin { FirstName = "admin2", UserName = "09184129532", PhoneNumber = "09184129532" },
                UserRoleName.Admin, "1234")
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