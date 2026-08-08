using Application.Interfaces.QueryServices;
using Application.Reservations.Dtos;
using Application.Reservations.Filters;
using Application.Reservations.Sorts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Models;
using Infrastructure.Common;
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
        ReservationFilterParameters? reservationFilterParameters,
        ReservationSortParameters? reservationSortParameters,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var result = await db.Reservations
            .ApplyFilter(reservationFilterParameters)
            .ApplySort(reservationSortParameters)
            .ProjectTo<ReservationDto>(configurationProvider)
            .PaginateAsync(paginationParameters, ct);

        return Result<PagedResult<ReservationDto>>.Success(result);
    }

    public async Task<Result<PagedResult<ReservationDto>>> GetAllByManagerIdAsync(
        Guid managerId,
        PaginationParameters paginationParameters,
        CancellationToken ct)
        => Result<PagedResult<ReservationDto>>.Success(await db.Reservations
            .Where(r => r.Room.Hotel.Manager != null
                        && r.Room.Hotel.Manager.Id == managerId)
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