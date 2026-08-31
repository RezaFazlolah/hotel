using Application.Rooms.Commands;
using FluentValidation;

namespace Application.Rooms.Validators;

public class UpdateRoomAsManagerCommandValidator
    : AbstractValidator<UpdateRoomAsManagerCommand>
{
    public UpdateRoomAsManagerCommandValidator()
    {
        Include(new UpdateRoomCommandValidatorBase());
    }
}