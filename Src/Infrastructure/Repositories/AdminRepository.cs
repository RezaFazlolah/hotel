using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SharedKernel.Common;
using SharedKernel.Extension;
using SharedKernel.Paging;

namespace Infrastructure.Repositories;

public class AdminRepository(
    AppDbContext db,
    IReservationRepository reservationRepository,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
    : UserRepository(db, userManager, roleManager), IAdminRepository
{
    public override async Task<Result<PagedResult<Reservation>>> GetAllReservationsAsync(Guid adminId,
        PaginationParameters paginationParameters, CancellationToken ct)
        => Result<PagedResult<Reservation>>.Success(await reservationRepository.GetAllAsQueryable()
           .ToPagedResultAsync(paginationParameters, ct));
}