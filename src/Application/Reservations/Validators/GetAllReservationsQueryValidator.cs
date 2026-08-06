using Application.Reservations.Filters;
using Application.Reservations.Queries;
using Application.Reservations.Sorts;
using FluentValidation;
using SharedKernel.Paginations;

namespace Application.Reservations.Validators;

// Future: use inheritance for GetAllHotelsQueryValidator, GetAllRoomsQueryValidator, GetAllReservationsQueryValidator 
public class GetAllReservationsQueryValidator
    : AbstractValidator<GetAllReservationsQuery>
{
    public GetAllReservationsQueryValidator(
        IValidator<ReservationFilterParameters> reservationFilterParametersValidator,
        IValidator<ReservationSortParameters> reservationSortParametersValidator,
        IValidator<PaginationParameters> paginationParametersValidator)
    {
        RuleFor(x => x.ReservationFilterParameters)
            .SetValidator(reservationFilterParametersValidator)
            .When(x => x.ReservationFilterParameters != null);

        RuleFor(x => x.ReservationSortParameters)
            .SetValidator(reservationSortParametersValidator)
            .When(x => x.ReservationSortParameters != null);
        
        RuleFor(x => x.PaginationParameters)
            .SetValidator(paginationParametersValidator);
    }
}