using Application.Reservations.Commands;
using FluentValidation;

namespace Application.Reservations.Validators;

public class UpdateReservationAsAdminCommandValidator
    :AbstractValidator<UpdateReservationAsAdminCommand>
{
    public UpdateReservationAsAdminCommandValidator()
    {
        Include(new UpdateReservationAsManagerCommandValidator());

        RuleFor(x => x.Status)
            .ValidReservationStatus();
    }
}