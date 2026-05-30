using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Requests.ReservationRequests;

public class GetAllReservations
    : IRequest<Result<PagedResult<Reservation>>>
{
    public PaginationParameters PaginationParameters { get; set; }
}