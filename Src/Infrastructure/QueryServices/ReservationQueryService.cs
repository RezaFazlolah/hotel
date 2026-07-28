using Application.Interfaces.QueryServices;
using Application.Reservations.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Infrastructure.QueryServices;

public class ReservationQueryService(
    AppDbContext db,
    IConfigurationProvider configurationProvider)
    : BaseQueryService<Reservation, ReservationDto>(db, configurationProvider),
        IReservationQueryService
{
    public async Task<Result<PagedResult<ReservationDto>>> GetAllByManagerIdAsync(
        Guid managerId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<ReservationDto>>.Success(await db.Reservations
            .Where(r => r.Room.Hotel.ManagerId == managerId)
            .ProjectTo<ReservationDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct));

    public async Task<Result<PagedResult<ReservationDto>>> GetAllByGuestIdAsync(
        Guid guestId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<ReservationDto>>.Success(await db.Reservations
            .Where(r => r.GuestId == guestId)
            .ProjectTo<ReservationDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct));
}