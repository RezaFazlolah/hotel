using Application.Hotels.Filters;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.Repositories;

public interface IHotelRepository
    : IBaseRepository<Guid, Hotel>
{
    Task<Result<Guid>> GetIdByManagerIdAsync(Guid managerId, CancellationToken ct);
    
    Task<Result<PagedResult<Hotel>>> GetAllAsync(
        HotelFilterParameters? hotelFilterParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);
}