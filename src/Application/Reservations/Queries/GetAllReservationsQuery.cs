using Application.Common;
using Application.Reservations.Dtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Reservations.Queries;

public record GetAllReservationsQuery
    : IRequest<Result<PagedResult<ReservationDto>>>
{
    public required PaginationParameters PaginationParameters { get; init; }
}