using Application.Common.Validators;
using FluentValidation;

namespace Application.Hotels.Queries.Validators;

// Future: use inheritance for GetAllHotelsQueryValidator, GetAllRoomsQueryValidator, GetAllReservationsQueryValidator 
public class GetAllHotelsQueryValidator
    : AbstractValidator<GetAllHotelsQuery>
{
    public GetAllHotelsQueryValidator()
    {
        RuleFor(x => x.PaginationParameters)
            .SetValidator(new PaginationParametersValidator());
    }
}