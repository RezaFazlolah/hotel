using Application.Reservations.Dtos;
using Domain.Models;

namespace Application.Interfaces.Services.Query;

public interface IReservationQueryService
    : IBaseQueryService<Reservation, ReservationDto>
{
}