using Application.Hotels.Dtos;
using Application.Hotels.Filters;
using Application.Hotels.Sorts;
using Application.Interfaces.QueryServices;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Models;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Infrastructure.QueryServices;

public class HotelQueryService(
    AppDbContext db,
    IConfigurationProvider configurationProvider)
    : BaseQueryService<Hotel, HotelDto>(db, configurationProvider),
        IHotelQueryService
{
    public async Task<Result<PagedResult<HotelDto>>> GetAllAsync(
        HotelFilterParameters? hotelFilterParameters,
        HotelSortParameters? hotelSortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var result = await db.Hotels
            .ApplyFilter(hotelFilterParameters)
            .ApplySort(hotelSortParameters)
            .ProjectTo<HotelDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct);

        return Result<PagedResult<HotelDto>>.Success(result);
    }
}