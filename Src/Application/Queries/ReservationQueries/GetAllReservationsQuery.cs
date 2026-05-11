using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Queries.ReservationQueries;

public class GetAllReservationsQuery
    : IRequest<Result<ICollection<Reservation>>>
{
}