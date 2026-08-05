using Application.Reservations.Dtos;
using Application.Reservations.Filters;
using Application.Reservations.Sorts;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.QueryServices;

public interface IReservationQueryService
    : IBaseQueryService<ReservationDto>
{
    Task<Result<PagedResult<ReservationDto>>> GetAllAsync(
        ReservationFilterParameters? hotelFilterParameters,
        ReservationSortParameters? hotelSortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);
    
    Task<Result<PagedResult<ReservationDto>>> GetAllByManagerIdAsync(
        Guid managerId,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<ReservationDto>>> GetAllByGuestIdAsync(
        Guid guestId,
        PaginationParameters paginationParameters,
        CancellationToken ct);
}