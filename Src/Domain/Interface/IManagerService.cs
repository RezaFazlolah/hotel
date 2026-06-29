using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Domain.Interface;

public interface IManagerService
    : IUserService
{
    // Result<IEnumerable<Guid>> GetRoomsIdAsync(Guid managerId, CancellationToken ct);

    Task<Result<PagedResult<Reservation>>> GetAllReservationsAsync(
        Guid managerId,
        PaginationParameters paginationParameters,
        CancellationToken ct);

    Task<Result<IEnumerable<Guid>>> GetAllRoomsIdAsync(Guid managerId, CancellationToken ct);
    Task<Result<bool>> ManagesRoomsAsync(Guid managerId, Guid roomId, CancellationToken ct);
}