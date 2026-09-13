using Api.Dtos.HotelDtos;
using FluentValidation;

namespace Api.Validators;

public class GetAllHotelsQueryDtoValidator
    : AbstractValidator<GetAllHotelsQueryDto>
{
    public GetAllHotelsQueryDtoValidator()
    {
        
    }
}