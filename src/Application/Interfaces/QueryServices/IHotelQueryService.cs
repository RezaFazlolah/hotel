using Application.Hotels.Dtos;
using Application.Hotels.Filters;
using Application.Hotels.Sorts;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.QueryServices;

public interface IHotelQueryService
    : IBaseQueryService<HotelDto>
{
    Task<Result<PagedResult<HotelDto>>> GetAllAsync(
        HotelFilterParameters? hotelFilterParameters,
        HotelSortParameters? hotelSortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<PagedResult<HotelDto>>> GetAllByManagerAsync(
        Guid managerId,
        HotelFilterParameters? filterParameters,
        HotelSortParameters sortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);
}