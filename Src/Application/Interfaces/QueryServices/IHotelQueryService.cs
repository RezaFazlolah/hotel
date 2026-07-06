using Application.Hotels.Dtos;
using Application.Hotels.Filters;
using AutoMapper;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Interfaces.QueryServices;

public interface IHotelQueryService
    : IBaseQueryService<Hotel, HotelDto>
{
    // public Task<Result<PagedResult<HotelDto>>> GetAllAsync(
        // HotelFilterParameters hotelFilterParameters,
        // PaginationParameters paginationParameters,
        // CancellationToken ct);
}