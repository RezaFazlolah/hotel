using Application.Reservations.Dtos;
using Application.Reservations.Filters;
using Application.Reservations.Sorts;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.QueryServices;

public interface IReservationQueryService
    : IQueryServiceBase<ReservationDto>
{
    Task<Result<PagedResult<ReservationDto>>> GetAllAsync(
        ReservationFilterParameters? filterParameters,
        ReservationSortParameters sortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<ReservationDto>>> GetAllByManagerAsync(
        Guid managerId,
        ReservationFilterParameters? filterParameters,
        ReservationSortParameters sortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<ReservationDto>>> GetAllByGuestAsync(
        Guid guestId,
        ReservationFilterParameters? filterParameters,
        ReservationSortParameters sortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);
}