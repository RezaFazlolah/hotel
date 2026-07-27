using Application.Common.Validators;
using Application.Hotels.Queries;
using FluentValidation;

namespace Application.Hotels.Validators;

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