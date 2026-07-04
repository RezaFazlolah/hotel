using Application.Hotels.Dtos;
using AutoMapper;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Filtering;
using SharedKernel.Paging;

namespace Application.Interfaces.QueryServices;

public interface IHotelQueryService
    : IBaseQueryService<Hotel, HotelDto>
{
    public Task<Result<PagedResult<HotelDto>>> GetAllAsync(
        HotelFilterParameters hotelFilterParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct);
}