using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IManagerRepository
    : IUserRepository
{
    Task<Result<Guid?>> GetHotelIdAsync(
        Guid managerId,
        CancellationToken ct);

    Task<Result<Guid>> GetIdByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct);
}