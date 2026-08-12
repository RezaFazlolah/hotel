using Application.Hotels.Dtos;
using Application.Hotels.Filters;
using Application.Hotels.Sorts;
using Application.Users.Dtos;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.QueryServices;

public interface IManagerQueryService
    : IBaseQueryService<ManagerDto>
{
    Task<Result<PagedResult<HotelDto>>> GetAllHotelsAsync(
        Guid managerId,
        HotelFilterParameters? hotelFilterParameters,
        HotelSortParameters hotelSortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);
}