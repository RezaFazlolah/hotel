using Application.Hotels.Dtos;
using Application.Hotels.Filters;
using Application.Interfaces.QueryServices;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Models;
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
    // private readonly IConfigurationProvider _configurationProvider = configurationProvider;

    public async Task<Result<HotelDto?>> GetByManagerIdAsync(
        Guid managerId,
        CancellationToken ct)
    {
        return Result<HotelDto?>.Success( 
            await db.Hotels
            .ProjectTo<HotelDto>(configurationProvider)
            .FirstOrDefaultAsync(h => h.ManagerId == managerId, ct));
    }

    public async Task<Result<PagedResult<HotelDto>>> GetAllAsync(
        PaginationParameters paginationParameters,
        HotelFilterParameters hotelFilterParameters,
        CancellationToken ct)
    {
        var result = await db.Hotels.AsQueryable()
            .ApplyFilter(hotelFilterParameters)
            .ProjectTo<HotelDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct);

        return Result<PagedResult<HotelDto>>.Success(result);
    }
}