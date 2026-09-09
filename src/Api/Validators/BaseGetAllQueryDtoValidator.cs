using Api.Dtos.HotelDtos;
using FluentValidation;

namespace Api.Validators;

public class BaseGetAllQueryDtoValidator
    : AbstractValidator<GetAllHotelsQueryDto>
{
    public BaseGetAllQueryDtoValidator()
    {
        RuleFor(x => x)
            .Must(x => x.PageNumber.HasValue == x.PageSize.HasValue)
            .WithMessage("PageNumber and PageSize must both be provided or omitted");
    }
}