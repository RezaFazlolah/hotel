using Domain.Models;
using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IHotelRepository
    : IBaseRepository<Guid, Hotel>
{
    Task<Result<Guid>> GetIdByManagerIdAsync(Guid managerId, CancellationToken ct);
}