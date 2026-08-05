using Application.Rooms.Sorts;
using FluentValidation;

namespace Application.Rooms.Validators;

public class RoomSortParametersValidator
    : AbstractValidator<RoomSortParameters>
{
    public RoomSortParametersValidator()
    {
        RuleFor(x => x.SortBy)
            .IsInEnum()
            .WithMessage("Room sort by is not valid");
    }
}