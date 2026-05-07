using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("UserType")
            .HasValue<Guest>("Guest")
            .HasValue<Manager>("Manager")
            .HasValue<Admin>("Admin");

        modelBuilder.Entity<Manager>()
            .HasOne(m => m.Hotel)
            .WithMany(h => h.Managers)
            .HasForeignKey(m => m.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Room>()
            .HasOne(r => r.Hotel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany(rm => rm.Reservations)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Guest)
            .WithMany(g => g.Reservations)
            .HasForeignKey(r => r.GuestId)
            .OnDelete(DeleteBehavior.Restrict);

        // ignore calculated properties
        modelBuilder.Entity<User>().Ignore(u => u.FullName);
        modelBuilder.Entity<Reservation>().Ignore(r => r.TotalPrice);

        // enum conversions
        modelBuilder.Entity<Room>()
            .Property(q => q.Type)
            .HasConversion<string>()
            .HasMaxLength(100);

        modelBuilder.Entity<Reservation>()
            .Property(q => q.Status)
            .HasConversion<string>()
            .HasMaxLength(100);

        // DB indices
        modelBuilder.Entity<Room>()
            .HasIndex(r => new { r.HotelId, r.Number })
            .IsUnique();

        modelBuilder.Entity<Reservation>()
            .HasIndex(r => new { r.CheckInDate, r.CheckOutDate });
    }
}