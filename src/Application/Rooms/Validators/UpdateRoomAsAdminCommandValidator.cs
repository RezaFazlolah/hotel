using Application.Rooms.Commands;
using FluentValidation;

namespace Application.Rooms.Validators;

public class UpdateRoomAsAdminCommandValidator
    : AbstractValidator<UpdateRoomAsAdminCommand>
{
    public UpdateRoomAsAdminCommandValidator()
    {
        Include(new UpdateRoomBaseCommandValidator());
    }
}