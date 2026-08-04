using Application.Hotels.Dtos;
using Application.Hotels.Filters;
using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.QueryServices;

public interface IHotelQueryService
    : IBaseQueryService<Hotel, HotelDto, HotelFilterParameters>
{
    public Task<Result<HotelDto?>> GetByManagerIdAsync(
            Guid managerId,
            CancellationToken ct);
}