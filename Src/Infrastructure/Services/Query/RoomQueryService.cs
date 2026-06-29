using Application.Interfaces.Services.Query;
using Application.Rooms.Dtos;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace Infrastructure.Services.Query;

public class RoomQueryService(
    AppDbContext db,
    IConfigurationProvider configurationProvider)
    : BaseQueryService<Room, RoomDto>(db, configurationProvider),
        IRoomQueryService
{
    public async Task<Result<ICollection<Guid>>> GetRoomsIdByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct)
        => Result<ICollection<Guid>>.Success(
            await db.Rooms
                .Where(r => r.HotelId == hotelId)
                .Select(r => r.Id)
                .ToListAsync(ct)
        );

    public async Task<Result<ICollection<Guid>>> GetRoomsIdByHotelIdsAsync(
        IEnumerable<Guid> hotelIds,
        CancellationToken ct)
        => Result<ICollection<Guid>>.Success(
            await db.Rooms
                .Where(r => hotelIds.Contains(r.HotelId))
                .Select(r => r.Id)
                .ToListAsync(ct)
        );
}