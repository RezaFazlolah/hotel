using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace Infrastructure.Repositories;

public class HotelRepository(AppDbContext db)
    : BaseRepository<Guid, Hotel>(db), IHotelRepository
{
    protected override IQueryable<Hotel> CustomContext()
        => db.Hotels
            .Include(h => h.Rooms)
            .Include(h => h.Managers);
}