using Application.Hotels.Dtos;
using SharedKernel.Paginations;

namespace Infrastructure.QueryServices;

public interface ManagerQueryService
{
    Task<PagedResult<HotelDto>> GetAllHotelsByManagerIdAsync();
}