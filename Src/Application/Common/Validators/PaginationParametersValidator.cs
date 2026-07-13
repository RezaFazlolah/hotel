using FluentValidation;
using SharedKernel.Paginations;

namespace Application.Common.Validators;

public class PaginationParametersValidator
    : AbstractValidator<PaginationParameters>
{
    // Future: read MaxPageSize from configuration 
    private const int MaxPageSize = 50;

    public PaginationParametersValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber must be at least 1");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, MaxPageSize)
            .WithMessage($"PageSize must be between 1 and {MaxPageSize}");
    }
}