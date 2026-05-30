using Application.Interfaces.Repositories;
using Application.Requests.ReservationRequests;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Paging;

namespace Application.Handlers.ReservationHandlers;

public class GetAllReservationsHandler(
    ICurrentUserRepository currentUserRepository,
    IUserRepository userRepository,
    IGuestRepository guestRepository,
    IManagerRepository managerRepository,
    IAdminRepository adminRepository)
    : IRequestHandler<GetAllReservations, Result<PagedResult<Reservation>>>
{
    public async Task<Result<PagedResult<Reservation>>> Handle(GetAllReservations request,
        CancellationToken ct)
    {
        var userId = currentUserRepository.Id;
        var roles = (await userRepository.GetRolesAsync(userId, ct)).Value;

        if (roles.Contains(UserRole.Admin))
            return await adminRepository.GetAllReservationsAsync(userId, request.PaginationParameters, ct);
        if (roles.Contains(UserRole.Manager))
            return await managerRepository.GetAllReservationsAsync(userId, request.PaginationParameters, ct);
        if (roles.Contains(UserRole.Guest))
            return await guestRepository.GetAllReservationsAsync(userId, request.PaginationParameters, ct);
        return Result<PagedResult<Reservation>>.Failure(new Error("user role not supported"));
    }
}