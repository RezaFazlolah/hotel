using Application.Rooms.Filters;
using Application.Rooms.Queries;
using Application.Rooms.Sorts;
using FluentValidation;
using SharedKernel.Paginations;

namespace Application.Rooms.Validators;

// Future: use inheritance for GetAllHotelsQueryValidator, GetAllRoomsQueryValidator, GetAllReservationsQueryValidator 
public class GetAllRoomsQueryValidator
    : AbstractValidator<GetAllRoomsQuery>
{
    public GetAllRoomsQueryValidator(
        IValidator<RoomFilterParameters> roomFilterParametersValidator,
        IValidator<RoomSortParameters> roomSortParametersValidator,
        IValidator<PaginationParameters> paginationParametersValidator)
    {
        RuleFor(x => x.RoomFilterParameters)
            .SetValidator(roomFilterParametersValidator)
            .When(x => x.RoomFilterParameters != null);

        RuleFor(x => x.RoomSortParameters)
            .SetValidator(roomSortParametersValidator);
        
        RuleFor(x => x.PaginationParameters)
            .SetValidator(paginationParametersValidator);
    }
}