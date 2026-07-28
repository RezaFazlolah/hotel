using Application.Common.Validators;
using Application.Hotels.Queries;
using FluentValidation;
using SharedKernel.Paginations;

namespace Application.Hotels.Validators;

// Future: use inheritance for GetAllHotelsQueryValidator, GetAllRoomsQueryValidator, GetAllReservationsQueryValidator 
public class GetAllHotelsQueryValidator
    : AbstractValidator<GetAllHotelsQuery>
{
    public GetAllHotelsQueryValidator(IValidator<PaginationParameters> paginationParametersValidator)
    {
        RuleFor(x => x.PaginationParameters)
            .SetValidator(paginationParametersValidator);
    }
}