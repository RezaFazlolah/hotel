using Application.Hotels.Dtos;
using Application.Hotels.Filters;
using Application.Hotels.Sorts;
using Application.Interfaces.QueryServices;
using Application.Users.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Models;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Infrastructure.QueryServices;

public class ManagerQueryService(
    AppDbContext db,
    IConfigurationProvider configurationProvider)
    : BaseQueryService<Manager, ManagerDto>(db, configurationProvider),
        IManagerQueryService
{
    public async Task<Result<PagedResult<HotelDto>>> GetAllHotelsAsync(
        Guid managerId,
        HotelFilterParameters? hotelFilterParameters,
        HotelSortParameters hotelSortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var result = await db.Managers
            .AsNoTracking()
            .Where(m => m.Id == managerId
                        && m.Hotel != null)
            .Select(m => m.Hotel!)
            .ApplyFilter(hotelFilterParameters)
            .ApplySort(hotelSortParameters)
            .ProjectTo<HotelDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct);

        return Result<PagedResult<HotelDto>>.Success(result);
    }
}