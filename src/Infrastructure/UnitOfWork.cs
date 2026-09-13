using Application.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure;

public class UnitOfWork(AppDbContext db)
    : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct)
        => db.SaveChangesAsync(ct);
}