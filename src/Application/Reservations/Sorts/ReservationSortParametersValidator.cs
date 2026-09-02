using FluentValidation;

namespace Application.Reservations.Sorts;

public class ReservationSortParametersValidator
    : AbstractValidator<ReservationSortParameters>
{
    public ReservationSortParametersValidator()
    {
        RuleFor(x => x.SortBy)
            .IsInEnum()
            .WithMessage($"SortBy must be {string.Join(", ", Enum.GetNames<ReservationSortBy>())}");
    }
}