using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure;

public class RoomConfiguration
    : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.Property(r => r.Type)
            .HasConversion<string>()
            .HasMaxLength(100);

        builder.HasIndex(r => new { r.HotelId, r.Number })
            .IsUnique();

        builder.HasOne(r => r.Hotel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class HotelConfiguration
    : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
    }
}

public class ReservationConfiguration
    : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.Property(r => r.Status)
            .HasConversion<string>()
            .HasMaxLength(100);

        builder.HasIndex(r => new { r.CheckInDate, r.CheckOutDate });

        builder.HasOne(r => r.Room)
            .WithMany(rm => rm.Reservations)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Guest)
            .WithMany(g => g.Reservations)
            .HasForeignKey(r => r.GuestId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class UserConfiguration
    : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Ignore(u => u.FullName);
    }
}

public class GuestConfiguration
    : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable("Guests");
    }
}

public class ManagerConfiguration
    : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.HasOne(m => m.Hotel)
            .WithMany(h => h.Managers)
            .HasForeignKey(m => m.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Managers");
    }
}

public class AdminConfiguration
    : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("Admins");
    }
}