using Api.Dtos.ReservationDtos;
using FluentValidation;

namespace Api.Validators;

public class GetAllReservationsQueryDtoValidator
:AbstractValidator<GetAllReservationsQueryDto>
{
    public GetAllReservationsQueryDtoValidator()
    {
        
    }
}