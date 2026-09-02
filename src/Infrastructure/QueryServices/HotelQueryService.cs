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
    : QueryServiceBase<Hotel, HotelDto>(db, configurationProvider),
        IHotelQueryService
{
    public async Task<Result<PagedResult<HotelDto>>> GetAllAsync(
        HotelFilterParameters? hotelFilterParameters,
        HotelSortParameters? hotelSortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var result = await db.Hotels
            .AsNoTracking()
            .ApplyFilter(hotelFilterParameters)
            .ApplySort(hotelSortParameters)
            .ProjectTo<HotelDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct);

        return Result<PagedResult<HotelDto>>.Success(result);
    }

    public async Task<Result<PagedResult<HotelDto>>> GetAllByManagerAsync(Guid managerId,
        HotelFilterParameters? filterParameters,
        HotelSortParameters sortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        await db.Reservations
            .Where(r => r.Room.Hotel.Manager != null
                        && r.Room.Hotel.Manager.Id == managerId)
            .ToListAsync(ct);

        var result = await db.Hotels
            .AsNoTracking()
            .Where(h => h.Manager != null
                        && h.Manager.Id == managerId)
            .ApplyFilter(filterParameters)
            .ApplySort(sortParameters)
            .ProjectTo<HotelDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct);

        return Result<PagedResult<HotelDto>>.Success(result);
    }
}