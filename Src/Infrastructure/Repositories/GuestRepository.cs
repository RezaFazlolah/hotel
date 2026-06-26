using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel.Common;
using SharedKernel.Extension;
using SharedKernel.Paging;

namespace Infrastructure.Repositories;

public class GuestRepository(
    AppDbContext db,
    IReservationRepository reservationRepository,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
    : UserRepository(db, userManager, roleManager), IGuestRepository
{
    public override async Task<Result<PagedResult<Reservation>>> GetAllReservationsAsync(Guid guestId,
        PaginationParameters paginationParameters, CancellationToken ct)
        => Result<PagedResult<Reservation>>.Success(await reservationRepository.GetAllAsQueryable()
            .Where(r => r.GuestId == guestId).ToPagedResultAsync(paginationParameters, ct));
}