using Application.Interfaces.Repositories;
using Domain.Interface;
using Domain.Models;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Services;

public class AdminService(IReservationRepository reservationRepository)
    : UserService, IAdminService
{
    public override async Task<Result<PagedResult<Reservation>>> GetAllReservationsAsync(Guid adminId,
        PaginationParameters paginationParameters, CancellationToken ct)
        => await reservationRepository.GetAllAsync(paginationParameters, ct);
}