using FluentValidation;

namespace Application.Hotels.Sorts;

public class HotelSortParametersValidator
    : AbstractValidator<HotelSortParameters>
{
    public HotelSortParametersValidator()
    {
        RuleFor(x => x.SortBy)
            .IsInEnum()
            .WithMessage("Hotel sort by is not valid");
    }
}