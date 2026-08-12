using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Entities;

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