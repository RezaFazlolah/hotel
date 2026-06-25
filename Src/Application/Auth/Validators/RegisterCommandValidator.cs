using Application.Auth.Commands;
using FluentValidation;

namespace Application.Auth.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required.")
            .Matches(@"^0\d{10}$").WithMessage("PhoneNumber format is invalid. example: 09123456789.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(3).WithMessage("Password must be at least 3 characters.")
            .MaximumLength(64).WithMessage("Password must not exceed 64 characters.");
    }
}