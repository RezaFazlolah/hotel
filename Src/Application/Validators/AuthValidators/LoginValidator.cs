using Application.Requests.AuthRequests;
using FluentValidation;

namespace Application.Validators.AuthValidators;

public class LoginValidator : AbstractValidator<Login>
{
    public LoginValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required")
            .Matches(@"^0\d{10}$").WithMessage("PhoneNumber format is invalid. example: 09123456789");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}