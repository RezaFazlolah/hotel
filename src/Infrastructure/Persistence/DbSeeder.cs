using Domain.Interfaces;
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
        var reservationService = serviceProvider.GetRequiredService<IReservationService>();
        var db = serviceProvider.GetRequiredService<AppDbContext>();

        // roles
        foreach (var role in Enum.GetNames<UserRole>())
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new Role(role));

        // users
        var users = new List<(User user, string role, string password)>
        {
            (new Guest
                {
                    FirstName = "guest1",
                    UserName = "09184129511",
                    PhoneNumber = "09184129511"
                },
                UserRoleAsString.Guest, "1234"),
            (new Guest
                {
                    FirstName = "guest2",
                    UserName = "09184129512",
                    PhoneNumber = "09184129512"
                },
                UserRoleAsString.Guest, "1234"),

            (new Manager
                {
                    FirstName = "manager1",
                    UserName = "09184129521",
                    PhoneNumber = "09184129521"
                },
                UserRoleAsString.Manager, "1234"),
            (new Manager
                {
                    FirstName = "manager2",
                    UserName = "09184129522",
                    PhoneNumber = "09184129522"
                },
                UserRoleAsString.Manager, "1234"),
            (new Manager
                {
                    FirstName = "manager3",
                    UserName = "09184129523",
                    PhoneNumber = "09184129523"
                },
                UserRoleAsString.Manager, "1234"),

            (new Admin
                {
                    FirstName = "admin1",
                    UserName = "09184129531",
                    PhoneNumber = "09184129531"
                },
                UserRoleAsString.Admin, "1234"),
            (new Admin
                {
                    FirstName = "admin2",
                    UserName = "09184129532",
                    PhoneNumber = "09184129532"
                },
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

        var seedHotel = !db.Hotels.Any();
        var seedRoom = seedHotel || !db.Rooms.Any();
        var seedReservation = seedRoom || !db.Reservations.Any();

        // hotels
        var hotels = new List<Hotel>
        {
            new() { Name = "Parsian", Address = "Hamadan", Rating = 3.8m, Manager = (Manager?)users[2].user },
            new() { Name = "Spinas", Address = "Tehran", Rating = 4.5m, Manager = (Manager)users[3].user },
            new() { Name = "Khatam", Address = "Isfahan", Rating = 4.9m, Manager = null },
        };

        if (seedHotel)
        {
            await db.Hotels.ExecuteDeleteAsync();
            await db.Hotels.AddRangeAsync(hotels);
        }

        // rooms
        var rooms = new List<Room>
        {
            new() { Number = 101, Type = RoomType.Normal, PricePerNight = 100, Hotel = hotels[0] },
            new() { Number = 302, Type = RoomType.Vip, PricePerNight = 500, Hotel = hotels[0] },
            new() { Number = 101, Type = RoomType.Vip, PricePerNight = 200, Hotel = hotels[1] },
            new() { Number = 711, Type = RoomType.Normal, PricePerNight = 400, Hotel = hotels[2] },
            new() { Number = 712, Type = RoomType.Normal, PricePerNight = 400, Hotel = hotels[2] },
            new() { Number = 713, Type = RoomType.Vip, PricePerNight = 700, Hotel = hotels[2] }
        };

        if (seedRoom)
        {
            await db.Rooms.ExecuteDeleteAsync();
            await db.Rooms.AddRangeAsync(rooms);
        }

        // reservations
        var reservations = new List<Reservation>
        {
            new()
            {
                CheckInDate = DateTimeOffset.Parse("2026-08-20T14:00:00+03:30"),
                CheckOutDate = DateTimeOffset.Parse("2026-08-22T14:00:00+03:30"),
                Status = ReservationStatus.Confirmed,
                GuestId = Guid.Empty,
                Guest = (Guest)users[0].user,
                RoomId = Guid.Empty,
                Room = rooms[0]
            },
            new()
            {
                CheckInDate = DateTimeOffset.Parse("2026-08-21T14:00:00+03:30"),
                CheckOutDate = DateTimeOffset.Parse("2026-08-24T14:00:00+03:30"),
                Status = ReservationStatus.Confirmed,
                GuestId = Guid.Empty,
                Guest = (Guest)users[0].user,
                RoomId = Guid.Empty,
                Room = rooms[3]
            },
            new()
            {
                CheckInDate = DateTimeOffset.Parse("2026-08-23T14:00:00+03:30"),
                CheckOutDate = DateTimeOffset.Parse("2026-08-26T14:00:00+03:30"),
                Status = ReservationStatus.Confirmed,
                GuestId = Guid.Empty,
                Guest = (Guest)users[1].user,
                RoomId = Guid.Empty,
                Room = rooms[0]
            }
        };

        foreach (var reservation in reservations)
            reservation.TotalPrice = reservationService.CalculatePrice(reservation.CheckInDate, reservation.CheckOutDate,
                reservation.Room.PricePerNight);

        if (seedReservation)
        {
            await db.Reservations.ExecuteDeleteAsync();
            await db.Reservations.AddRangeAsync(reservations);
        }

        await db.SaveChangesAsync();
    }
}