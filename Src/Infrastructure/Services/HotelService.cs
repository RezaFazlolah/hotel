using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Filtering;

namespace Infrastructure.Services;

public class HotelService(AppDbContext context)
    : BaseService<Guid, Hotel>(context), IHotelService
{
    public async Task<Result<ICollection<Room>>> GetRoomsAsync(Guid hotelId, CancellationToken ct)
        => await GetRoomsAsync([hotelId], ct);

    public async Task<Result<ICollection<Room>>> GetRoomsAsync(IEnumerable<Guid> hotelsId, CancellationToken ct)
        // implement with RoomService's GetRooms() with proper filter instead of this
        => Result<ICollection<Room>>.Success(await context.Rooms.Where(r => hotelsId.Contains(r.HotelId))
            .ToListAsync(ct));

    public async Task<Result<ICollection<Guid>>> GetRoomsIdAsync(Guid hotelId, CancellationToken ct)
        => await GetRoomsIdAsync([hotelId], ct);

    public async Task<Result<ICollection<Guid>>> GetRoomsIdAsync(IEnumerable<Guid> hotelsId, CancellationToken ct)
        // implement with RoomService's GetRoomsId() with proper filter instead of this
        => Result<ICollection<Guid>>.Success(await context.Rooms.Where(r => hotelsId.Contains(r.HotelId))
            .Select(r => r.Id).ToListAsync(ct));

    public async Task<Result<ICollection<Reservation>>> GetReservationsAsync(Guid hotelId, CancellationToken ct)
        => await GetReservationsAsync([hotelId], ct);

    public async Task<Result<ICollection<Reservation>>> GetReservationsAsync(IEnumerable<Guid> hotelsId,
            CancellationToken ct)
        // implement with ReservationService's GetReservations() with proper filter instead of this
    {
        var roomsId = (await GetRoomsIdAsync(hotelsId, ct)).Value;
        return Result<ICollection<Reservation>>.Success(await context.Reservations.Where(r => roomsId.Contains(r.Id))
            .ToListAsync(ct));
    }

    public async Task<Result<bool>> RoomNumberExistsAsync(int roomNumber, Guid hotelId,
            CancellationToken cancellationToken)
        // implement with RoomService's RoomExists() with proper filter instead of this
        => Result<bool>.Success((await context.Rooms.AnyAsync(r => r.HotelId == hotelId && r.Number == roomNumber,
            cancellationToken: cancellationToken)));

    protected override IQueryable<Hotel> CustomContext()
    {
        throw new NotImplementedException();
        // var query = context.Hotels
        //     .Include(h => h.Rooms)
        //     .Include(h => h.Managers);
        //
        // query.        
        //
        // return query;
    }

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