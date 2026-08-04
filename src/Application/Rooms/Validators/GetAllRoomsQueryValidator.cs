using Application.Common.Validators;
using Application.Rooms.Filters;
using Application.Rooms.Queries;
using FluentValidation;
using SharedKernel.Paginations;

namespace Application.Rooms.Validators;

// Future: use inheritance for GetAllHotelsQueryValidator, GetAllRoomsQueryValidator, GetAllReservationsQueryValidator 
public class GetAllRoomsQueryValidator
    : AbstractValidator<GetAllRoomsQuery>
{
    public GetAllRoomsQueryValidator(
        IValidator<PaginationParameters> paginationParametersValidator,
        IValidator<RoomFilterParameters> roomFilterParametersValidator)
    {
        RuleFor(x => x.PaginationParameters)
            .SetValidator(paginationParametersValidator);

        RuleFor(x => x.RoomFilterParameters)
            .SetValidator(roomFilterParametersValidator)
            .When(x => x.RoomFilterParameters != null);
    }
}