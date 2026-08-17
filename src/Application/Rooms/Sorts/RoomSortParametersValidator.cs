using FluentValidation;

namespace Application.Rooms.Sorts;

public class RoomSortParametersValidator
    : AbstractValidator<RoomSortParameters>
{
    public RoomSortParametersValidator()
    {
        RuleFor(x => x.SortBy)
            .IsInEnum()
            .WithMessage("SortBy is not valid");
    }
}