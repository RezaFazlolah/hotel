using Application.Interfaces.Repositories;
using Domain.Interface;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Services;

public class GuestService(IGuestRepository guestRepository)
    : UserService, IGuestService
{
    public override Task<Result<PagedResult<Reservation>>> GetAllReservationsAsync(Guid guestId,
        PaginationParameters paginationParameters, CancellationToken ct)
        => guestRepository.GetAllReservationsAsync(guestId, paginationParameters, ct);
}