using Application.Reservations.Dtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Reservations.Queries;

public record GetAllReservationsQuery(PaginationParameters PaginationParameters)
    : IRequest<Result<PagedResult<ReservationDto>>>;