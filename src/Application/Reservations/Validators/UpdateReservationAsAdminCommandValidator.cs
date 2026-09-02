using Application.Reservations.Commands;
using FluentValidation;

namespace Application.Reservations.Validators;

public class UpdateReservationAsAdminCommandValidator
    :AbstractValidator<UpdateReservationAsAdminCommand>
{
    public UpdateReservationAsAdminCommandValidator()
    {
        Include(new UpdateReservationBaseCommandValidator());

        RuleFor(x => x.Status)
            .ValidReservationStatus();
    }
}