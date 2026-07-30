using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Reservations.Commands;
using Application.Reservations.Dtos;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Reservations.Handlers;

// Bug: i get error, it has something to do with DB lock.
// i spent about an hour trying to debug it and nuked DB multiple times, but i couldn't solve it.
// when i uncomment "!(r.CheckOutDate < checkInDate || checkOutDate < r.CheckInDate)" at ReservationRepository.IsReservedAsync(), my problem is solved.
// come back later
public class InsertReservationCommandHandler(
    IReservationRepository reservationRepository,
    IRoomRepository roomRepository,
    IGuestRepository guestRepository,
    ICurrentUserService currentUserService,
    IManagerService managerService,
    IReservationService reservationService,
    IMapper mapper)
    : IRequestHandler<InsertReservationCommand, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(
        InsertReservationCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"insert reservation for guest {request.GuestId} and room {request.RoomId} failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<ReservationDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            var errors = new List<Error>();

            var guestExists = await guestRepository.ExistsAsync(request.GuestId, ct);
            if (!guestExists)
                errors.Add(new Error($"guest not found"));

            var roomExists = await roomRepository.ExistsAsync(request.RoomId, ct);
            if (!roomExists)
                errors.Add(new Error($"room not found"));

            if (errors.Any())
                return Result<ReservationDto>.Failure(errors.Prepend(rootError));
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            var errors = new List<Error>();

            var guestExists = await guestRepository.ExistsAsync(request.GuestId, ct);
            if (!guestExists)
                errors.Add(new Error($"guest not found"));

            var managesRoom = await managerService.ManagesRoomAsync(currentUserInfo.id, request.RoomId, ct);
            if (!managesRoom)
                errors.Add(new Error($"room not found"));

            if (errors.Any())
                return Result<ReservationDto>.Failure(errors.Prepend(rootError));
        }
        else if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            var roomExists = await roomRepository.ExistsAsync(request.RoomId, ct);
            if (!roomExists)
                return Result<ReservationDto>.Failure([rootError, new Error($"room not found")]);

            // for Guest, ignore request's GuestId
            request = request with { GuestId = request.GuestId };
        }
        else
            return Result<ReservationDto>.Forbidden(rootError);

        var isReserved = await reservationRepository.IsRoomReservedAsync(request.RoomId, request.CheckInDate,
            request.CheckOutDate, ct);
        if (isReserved)
            return Result<ReservationDto>.Failure([rootError, new Error($"room is reserved")]);

        var reservation = mapper.Map<Reservation>(request);

        var totalPriceResult = await reservationService.CalculatePriceAsync(reservation, ct);
        if (!totalPriceResult.Succeeded)
            return Result<ReservationDto>.Failure(totalPriceResult.Errors.Prepend(rootError));
        var totalPrice = totalPriceResult.Value;

        reservation.TotalPrice = totalPrice;
        reservation.Status = ReservationStatus.Confirmed;

        var reservationCreateResult = await reservationRepository.InsertAsync(reservation, ct);
        return reservationCreateResult.Succeeded
            ? mapper.Map<Result<ReservationDto>>(reservation)
            : Result<ReservationDto>.Failure(reservationCreateResult.Errors.Prepend(rootError));
    }
}