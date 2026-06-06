using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Domain.Interface;

public interface IUserService
{
    Task<Result<PagedResult<Reservation>>> GetAllReservationsAsync(Guid userId,
        PaginationParameters paginationParameters, CancellationToken ct);
}