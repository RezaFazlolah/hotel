using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters;

public class DateTimeOffsetToUtcConverter
    : ValueConverter<DateTimeOffset, DateTimeOffset>
{
    public DateTimeOffsetToUtcConverter()
        : base(v => v.ToUniversalTime(), v => v)
    {
    }
}