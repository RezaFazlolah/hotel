using Application.Hotels.Dtos;
using Domain.Models;

namespace Application.Interfaces.QueryServices;

public interface IHotelQueryService
    : IBaseQueryService<Hotel, HotelDto>
{
    // public Task<Result<PagedResult<HotelDto>>> GetAllAsync(
        // HotelFilterParameters hotelFilterParameters,
        // PaginationParameters paginationParameters,
        // CancellationToken ct);
}