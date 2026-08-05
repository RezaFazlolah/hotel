using Application.Hotels.Sorts;
using FluentValidation;

namespace Application.Hotels.Validators;

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