using Application.Common.Validators;
using Application.Reservations.Queries;
using FluentValidation;

namespace Application.Reservations.Validators;

// Future: use inheritance for GetAllHotelsQueryValidator, GetAllRoomsQueryValidator, GetAllReservationsQueryValidator 
public class GetAllReservationsQueryValidator
    : AbstractValidator<GetAllReservationsQuery>
{
    public GetAllReservationsQueryValidator()
    {
        RuleFor(x => x.PaginationParameters)
            .SetValidator(new PaginationParametersValidator());
    }
}