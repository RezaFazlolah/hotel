using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Entities;

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