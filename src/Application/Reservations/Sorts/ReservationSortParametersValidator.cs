using FluentValidation;

namespace Application.Reservations.Sorts;

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