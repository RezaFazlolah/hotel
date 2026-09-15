using Application.Hotels.Configurations;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence.Configurations.Entities;

public class HotelConfiguration(IOptions<HotelSettings> hotelOptions)
    : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        var hotelSettings = hotelOptions.Value;
        
        builder.Property(h => h.Name).HasMaxLength(hotelSettings.MaxNameLength);
        builder.Property(h => h.Address).HasMaxLength(hotelSettings.MaxAddressLength);
    }
}