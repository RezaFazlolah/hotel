using Application.Reservations.Dtos;
using Domain.Models;

namespace Application.Interfaces.QueryServices;

public interface IReservationQueryService
    : IBaseQueryService<Reservation, ReservationDto>
{
}