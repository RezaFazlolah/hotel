using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Entities;

public class ManagerConfiguration
    : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.HasOne(m => m.Hotel)
            .WithOne(h => h.Manager)
            .HasForeignKey<Manager>(m => m.HotelId);
    }
}