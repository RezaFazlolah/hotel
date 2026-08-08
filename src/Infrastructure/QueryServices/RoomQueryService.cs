using Application.Interfaces.QueryServices;
using Application.Rooms.Dtos;
using Application.Rooms.Filters;
using Application.Rooms.Sorts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Models;
using Infrastructure.Common;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Infrastructure.QueryServices;

public class RoomQueryService(
    AppDbContext db,
    IConfigurationProvider configurationProvider)
    : BaseQueryService<Room, RoomDto>(db, configurationProvider),
        IRoomQueryService
{
    public async Task<Result<PagedResult<RoomDto>>> GetAllAsync(
        RoomFilterParameters? roomFilterParameters,
        RoomSortParameters? roomSortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var result = await db.Rooms
            .ApplyFilter(roomFilterParameters)
            .ApplySort(roomSortParameters)
            .ProjectTo<RoomDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct);

        return Result<PagedResult<RoomDto>>.Success(result);
    }

    public async Task<Result<PagedResult<RoomDto>>> GetAllByHotelIdAsync(
        Guid hotelId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<RoomDto>>.Success(
            await db.Rooms
                .Where(r => r.HotelId == hotelId)
                .ProjectTo<RoomDto>(configurationProvider)
                .PaginateAsync(paginationParameters, ct));

    public async Task<Result<PagedResult<RoomDto>>> GetAllByManagerIdAsync(
        Guid managerId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<RoomDto>>.Success(
            await db.Rooms
                .Where(r => r.Hotel.Manager != null
                            && r.Hotel.Manager.Id == managerId)
                .ProjectTo<RoomDto>(configurationProvider)
                .PaginateAsync(paginationParameters, ct));
}