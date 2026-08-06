using Application.Hotels.Filters;
using Application.Hotels.Queries;
using Application.Hotels.Sorts;
using FluentValidation;
using SharedKernel.Paginations;

namespace Application.Hotels.Validators;

// Future: use inheritance for GetAllHotelsQueryValidator, GetAllRoomsQueryValidator, GetAllReservationsQueryValidator 
public class GetAllHotelsQueryValidator
    : AbstractValidator<GetAllHotelsQuery>
{
    public GetAllHotelsQueryValidator(
        IValidator<HotelFilterParameters> hotelFilterParametersValidator,
        IValidator<HotelSortParameters> hotelSortParametersValidator,
        IValidator<PaginationParameters> paginationParametersValidator)
    {
        RuleFor(x=>x.HotelFilterParameters)
            .SetValidator(hotelFilterParametersValidator)
            .When(x => x.HotelFilterParameters != null);

        RuleFor(x => x.HotelSortParameters)
            .SetValidator(hotelSortParametersValidator)
            .When(x => x.HotelSortParameters!= null);
        
        RuleFor(x => x.PaginationParameters)
            .SetValidator(paginationParametersValidator);
    }
}