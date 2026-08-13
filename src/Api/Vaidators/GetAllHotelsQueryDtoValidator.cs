using Api.Dtos.HotelDtos;
using FluentValidation;

namespace Api.Vaidators;

public class GetAllHotelsQueryDtoValidator
    : AbstractValidator<GetAllHotelsQueryDto>
{
    public GetAllHotelsQueryDtoValidator()
    {
        RuleFor(x => x)
            .Must(x =>
                x.PageNumber.HasValue == x.PageSize.HasValue)
            .WithMessage(
                "PageNumber and PageSize must either both be provided or both be omitted.");
    }
}