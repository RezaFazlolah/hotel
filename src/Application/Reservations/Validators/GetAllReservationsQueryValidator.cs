using Application.Common.Validators;
using Application.Reservations.Filters;
using Application.Reservations.Queries;
using FluentValidation;
using SharedKernel.Paginations;

namespace Application.Reservations.Validators;

// Future: use inheritance for GetAllHotelsQueryValidator, GetAllRoomsQueryValidator, GetAllReservationsQueryValidator 
public class GetAllReservationsQueryValidator
    : AbstractValidator<GetAllReservationsQuery>
{
    public GetAllReservationsQueryValidator(
        IValidator<PaginationParameters> paginationParametersValidator,
        IValidator<ReservationFilterParameters> reservationFilterParametersValidator
    )
    {
        RuleFor(x => x.PaginationParameters)
            .SetValidator(paginationParametersValidator);

        RuleFor(x => x.ReservationFilterParameters)
            .SetValidator(reservationFilterParametersValidator)
            .When(x => x.ReservationFilterParameters != null);
    }
}