using Domain.Interface;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Services;

public abstract class UserService
    : IUserService
{
    // public abstract Task<Result<PagedResult<Reservation>>> GetAllReservationsAsync(Guid userId,
    //     PaginationParameters paginationParameters, CancellationToken ct);
}