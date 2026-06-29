using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace Infrastructure.Repositories;

public class HotelRepository(AppDbContext db)
    : BaseRepository<Guid, Hotel>(db), IHotelRepository
{
    public async Task<Result<ICollection<Room>>> GetRoomsAsync(Guid hotelId, CancellationToken ct)
        // implement with RoomRepository's GetRooms() with proper filter instead of this
        => Result<ICollection<Room>>.Success(await db.Rooms.Where(r => r.HotelId == hotelId)
            .ToListAsync(ct));
    
    // public async Task<Result<ICollection<Room>>> GetRoomsAsync(IEnumerable<Guid> hotelsId, CancellationToken ct)
    //     // implement with RoomRepository's GetRooms() with proper filter instead of this
    // => throw new NotImplementedException();

    public async Task<Result<ICollection<Guid>>> GetRoomsIdAsync(Guid hotelId, CancellationToken ct)
        // implement with RoomRepository's GetRoomsId() with proper filter instead of this
        => Result<ICollection<Guid>>.Success(await db.Rooms.Where(r => r.HotelId == hotelId)
            .Select(r => r.Id).ToListAsync(ct));

    public async Task<Result<ICollection<Reservation>>> GetReservationsAsync(Guid hotelId,
            CancellationToken ct)
        // implement with ReservationRepository's GetReservations() with proper filter instead of this
    {
        var roomsId = (await GetRoomsIdAsync(hotelId, ct)).Value;
        return Result<ICollection<Reservation>>.Success(await db.Reservations.Where(r => roomsId.Contains(r.Id))
            .ToListAsync(ct));
    }

    public async Task<Result<bool>> RoomNumberExistsAsync(int roomNumber, Guid hotelId,
        CancellationToken cancellationToken)
    {
        if (!await ExistsAsync(hotelId, cancellationToken))
            return Result<bool>.Failure(new Error($"hotel {hotelId} not found."));

        var roomExists = await db.Rooms.AnyAsync(r => r.HotelId == hotelId && r.Number == roomNumber,
            cancellationToken: cancellationToken);
        return Result<bool>.Success(roomExists);
    }

    protected override IQueryable<Hotel> CustomContext()
        => db.Hotels
            .Include(h => h.Rooms)
            .Include(h => h.Managers);

    // protected override IQueryable<Hotel> Filter(IQueryable<Hotel> query, HotelFilterParameters filterParameters)
    // {
    //     // query = base.Filter(query, filterParameters);
    //
    //     if (!string.IsNullOrWhiteSpace(filterParameters.Name))
    //         query = query.Where(h => h.Name.Contains(filterParameters.Name));
    //
    //     if (!string.IsNullOrWhiteSpace(filterParameters.Address))
    //         query = query.Where(h => h.Address.Contains(filterParameters.Address));
    //
    //     if (filterParameters.MinRating.HasValue)
    //         query = query.Where(h => h.Rating >= filterParameters.MinRating.Value);
    //
    //     if (filterParameters.MaxRating.HasValue)
    //         query = query.Where(h => h.Rating <= filterParameters.MaxRating.Value);
    //
    //     return query;
    // }
}