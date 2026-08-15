using Application.Interfaces.QueryServices;
using Application.Reservations.Dtos;
using Application.Reservations.Filters;
using Application.Reservations.Sorts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Models;
using Infrastructure.Common;
using Infrastructure.Persistence;
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
    public async Task<Result<PagedResult<ReservationDto>>> GetAllAsync(
        ReservationFilterParameters? filterParameters,
        ReservationSortParameters sortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var result = await db.Reservations
            .AsNoTracking()
            .ApplyFilter(filterParameters)
            .ApplySort(sortParameters)
            .ProjectTo<ReservationDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct);

        return Result<PagedResult<ReservationDto>>.Success(result);
    }

    public async Task<Result<PagedResult<ReservationDto>>> GetAllByManagerAsync(
        Guid managerId,
        ReservationFilterParameters? filterParameters,
        ReservationSortParameters sortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<ReservationDto>>.Success(await db.Reservations
            .AsNoTracking()
            .Where(r => r.Room.Hotel.Manager != null
                        && r.Room.Hotel.Manager.Id == managerId)
            .ApplyFilter(filterParameters)
            .ApplySort(sortParameters)
            .ProjectTo<ReservationDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct));

    public async Task<Result<PagedResult<ReservationDto>>> GetAllByGuestAsync(
        Guid guestId,
        ReservationFilterParameters? filterParameters,
        ReservationSortParameters sortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<ReservationDto>>.Success(await db.Reservations
            .AsNoTracking()
            .Where(r => r.GuestId == guestId)
            .ApplyFilter(filterParameters)
            .ApplySort(sortParameters)
            .ProjectTo<ReservationDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct));
}