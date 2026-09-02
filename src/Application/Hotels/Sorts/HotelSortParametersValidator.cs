using FluentValidation;

namespace Application.Hotels.Sorts;

public class HotelSortParametersValidator
    : AbstractValidator<HotelSortParameters>
{
    public HotelSortParametersValidator()
    {
        RuleFor(x => x.SortBy)
            .IsInEnum()
            .WithMessage($"SortBy {string.Join(", ", Enum.GetNames<HotelSortBy>())}");
    }
}