using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var db = serviceProvider.GetRequiredService<AppDbContext>();

        // hotel
        var hotels = new List<Hotel>
        {
            new()
            {
                Name = "Parsian",
                Address = "Hamadan",
                Rating = 3.8m
            },
            new()
            {
                Name = "Spinas",
                Address = "Tehran",
                Rating = 4.5m
            },
            new()
            {
                Name = "Khatam",
                Address = "Isfahan",
                Rating = 4.9m
            },
        };

        if (!db.Hotels.Any())
        {
            await db.Hotels.AddRangeAsync(hotels);
            await db.SaveChangesAsync();
        }

        // room
        var hotelIds = await db.Hotels
            .Select(h => h.Id)
            .ToListAsync();

        var rooms = new List<Room>
        {
            new Room { Number = 101, Type = RoomType.Normal, PricePerNight = 100, HotelId = hotelIds[0] },
            new Room { Number = 302, Type = RoomType.Vip, PricePerNight = 500, HotelId = hotelIds[0] },
            new Room { Number = 101, Type = RoomType.Vip, PricePerNight = 200, HotelId = hotelIds[1] },
            new Room { Number = 711, Type = RoomType.Normal, PricePerNight = 400, HotelId = hotelIds[2] },
            new Room { Number = 712, Type = RoomType.Normal, PricePerNight = 400, HotelId = hotelIds[2] },
            new Room { Number = 713, Type = RoomType.Vip, PricePerNight = 700, HotelId = hotelIds[2] },
        };

        if (!db.Rooms.Any())
        {
            await db.Rooms.AddRangeAsync(rooms);
            await db.SaveChangesAsync();
        }

        // roles
        foreach (var role in Enum.GetNames<UserRole>())
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new Role(role));

        // users
        var users = new List<(User user, string role, string password)>
        {
            (new Guest { FirstName = "guest1", UserName = "09184129511", PhoneNumber = "09184129511" },
                UserRoleAsString.Guest, "1234"),
            (new Guest { FirstName = "guest2", UserName = "09184129512", PhoneNumber = "09184129512" },
                UserRoleAsString.Guest, "1234"),

            (new Manager { FirstName = "manager1", UserName = "09184129521", PhoneNumber = "09184129521", HotelId = hotelIds[0]},
                UserRoleAsString.Manager, "1234"),
            (new Manager { FirstName = "manager2", UserName = "09184129522", PhoneNumber = "09184129522", HotelId = hotelIds[1]},
                UserRoleAsString.Manager, "1234"),
            (new Manager { FirstName = "manager3", UserName = "09184129523", PhoneNumber = "09184129523", HotelId = hotelIds[2]},
                UserRoleAsString.Manager, "1234"),

            (new Admin { FirstName = "admin1", UserName = "09184129531", PhoneNumber = "09184129531" },
                UserRoleAsString.Admin, "1234"),
            (new Admin { FirstName = "admin2", UserName = "09184129532", PhoneNumber = "09184129532" },
                UserRoleAsString.Admin, "1234")
        };

        foreach (var user in users)
        {
            if (await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == user.user.PhoneNumber) == null)
            {
                var result = await userManager.CreateAsync(user.user, user.password);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user.user, user.role);
            }
        }
    }
}