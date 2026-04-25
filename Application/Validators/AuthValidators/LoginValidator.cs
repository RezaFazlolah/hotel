using Application.Commands.AuthCommands;
using FluentValidation;

namespace Application.Validators.AuthValidators;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required");
        // .Matches(@"^\+?[1-9]\d{9,14}$")
        // .WithMessage("PhoneNumber format is invalid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
