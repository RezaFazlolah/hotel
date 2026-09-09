using Api.Dtos.RoomDtos;
using FluentValidation;

namespace Api.Validators;

public class GetAllRoomsQueryDtoValidator
:AbstractValidator<GetAllRoomsQueryDto>
{
    public GetAllRoomsQueryDtoValidator()
    {
        
    }
}