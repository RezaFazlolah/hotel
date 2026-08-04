using Application.Reservations.Dtos;
using Application.Reservations.Filters;
using Application.Reservations.Sorts;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.QueryServices;

public interface IReservationQueryService
    : IBaseQueryService<Reservation, ReservationDto, ReservationFilterParameters, ReservationSortParameters>
{
    Task<Result<PagedResult<ReservationDto>>> GetAllByManagerIdAsync(
        Guid managerId,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<ReservationDto>>> GetAllByGuestIdAsync(
        Guid guestId,
        PaginationParameters paginationParameters,
        CancellationToken ct);
}