using Application.Reservations.Sorts;
using FluentValidation;

namespace Application.Reservations.Validators;

public class ReservationSortParametersValidator
    : AbstractValidator<ReservationSortParameters>
{
    public ReservationSortParametersValidator()
    {
        RuleFor(x => x.SortBy)
            .IsInEnum()
            .WithMessage("Reservation sort by is not valid");
    }
}