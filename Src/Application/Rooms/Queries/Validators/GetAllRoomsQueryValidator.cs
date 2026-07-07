using Application.Common.Validators;
using FluentValidation;

namespace Application.Rooms.Queries.Validators;

// Future: use inheritance for GetAllHotelsQueryValidator, GetAllRoomsQueryValidator, GetAllReservationsQueryValidator 
public class GetAllRoomsQueryValidator
    : AbstractValidator<GetAllRoomsQuery>
{
    public GetAllRoomsQueryValidator()
    {
        RuleFor(x => x.PaginationParameters)
            .SetValidator(new PaginationParametersValidator());
    }
}