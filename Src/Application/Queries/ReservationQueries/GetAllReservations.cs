using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Queries.ReservationQueries;

public class GetAllReservations
    : IRequest<Result<ICollection<Reservation>>>
{
    public PaginationParameters PaginationParameters { get; set; }
}