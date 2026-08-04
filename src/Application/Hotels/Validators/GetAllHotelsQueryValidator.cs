using Application.Hotels.Filters;
using Application.Hotels.Queries;
using FluentValidation;
using SharedKernel.Paginations;

namespace Application.Hotels.Validators;

// Future: use inheritance for GetAllHotelsQueryValidator, GetAllRoomsQueryValidator, GetAllReservationsQueryValidator 
public class GetAllHotelsQueryValidator
    : AbstractValidator<GetAllHotelsQuery>
{
    public GetAllHotelsQueryValidator(
        IValidator<PaginationParameters> paginationParametersValidator,
        IValidator<HotelFilterParameters> hotelFilterParametersValidator)
    {
        RuleFor(x => x.PaginationParameters)
            .SetValidator(paginationParametersValidator);
        
        RuleFor(x=>x.HotelFilterParameters)
            .SetValidator(hotelFilterParametersValidator)
            .When(x => x.HotelFilterParameters != null);
    }
}