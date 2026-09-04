using Application.Interfaces.QueryServices;
using Application.Rooms.Dtos;
using Application.Rooms.Filters;
using Application.Rooms.Sorts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Models;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Infrastructure.QueryServices;

public class RoomQueryService(
    AppDbContext db,
    IConfigurationProvider configurationProvider)
    : BaseQueryService<Domain.Models.Room, RoomDto>(db, configurationProvider),
        IRoomQueryService
{
    public async Task<Result<PagedResult<RoomDto>>> GetAllAsync(
        RoomFilterParameters? roomFilterParameters,
        RoomSortParameters? roomSortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var result = await db.Rooms
            .AsNoTracking()
            .ApplyFilter(roomFilterParameters)
            .ApplySort(roomSortParameters)
            .ProjectTo<RoomDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct);

        return Result<PagedResult<RoomDto>>.Success(result);
    }

    public async Task<Result<PagedResult<RoomDto>>> GetAllByHotelAsync(
        Guid hotelId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<RoomDto>>.Success(
            await db.Rooms
                .AsNoTracking()
                .Where(r => r.HotelId == hotelId)
                .ProjectTo<RoomDto>(configurationProvider)
                .PaginateAsync(paginationParameters, ct));

    public async Task<Result<PagedResult<RoomDto>>> GetAllByManagerAsync(
        Guid managerId,
        RoomFilterParameters? filterParameters,
        RoomSortParameters sortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<RoomDto>>.Success(
            await db.Rooms
                .AsNoTracking()
                .Where(r => r.Hotel.Manager != null
                            && r.Hotel.Manager.Id == managerId)
                .ApplyFilter(filterParameters)
                .ApplySort(sortParameters)
                .ProjectTo<RoomDto>(configurationProvider)
                .PaginateAsync(paginationParameters, ct));
}