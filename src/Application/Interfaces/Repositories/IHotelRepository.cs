using Application.Hotels.Filters;
using Application.Hotels.Sorts;
using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IHotelRepository
    : IRepositoryBase<Guid, Hotel>
{
    Task<Result<Guid?>> GetManagerIdAsync(
        Guid hotelId,
        CancellationToken ct);
    
    Task<Result<decimal>> GetRatingAsync(
        Guid hotelId,
        CancellationToken ct);
}