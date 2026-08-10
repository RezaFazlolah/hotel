using SharedKernel.Common;

namespace Application.Interfaces.Repositories;

public interface IManagerRepository
    : IUserRepository
{
    Task<Result<Guid?>> GetHotelIdAsync(
        Guid managerId,
        CancellationToken ct);

    Task<bool> ManagesHotel(
        Guid managerId,
        Guid hotelId,
        CancellationToken ct);
}