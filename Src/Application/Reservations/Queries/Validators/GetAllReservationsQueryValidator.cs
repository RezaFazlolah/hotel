using Application.Common.Validators;
using FluentValidation;

namespace Application.Reservations.Queries.Validators;

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