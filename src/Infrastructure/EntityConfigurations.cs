using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure;

public class HotelConfiguration
    : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
    }
}

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

        // builder.HasIndex(r => new { r.Id, r.CheckInDate, r.CheckOutDate });
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
            .WithOne(h => h.Manager)
            .HasForeignKey<Manager>(m => m.HotelId);

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