using Application.Common.Validators;
using Application.Reservations.Queries;
using FluentValidation;
using SharedKernel.Paginations;

namespace Application.Reservations.Validators;

// Future: use inheritance for GetAllHotelsQueryValidator, GetAllRoomsQueryValidator, GetAllReservationsQueryValidator 
public class GetAllReservationsQueryValidator
    : AbstractValidator<GetAllReservationsQuery>
{
    public GetAllReservationsQueryValidator(IValidator<PaginationParameters> paginationParametersValidator)
    {
        RuleFor(x => x.PaginationParameters)
            .SetValidator(paginationParametersValidator);
    }
}