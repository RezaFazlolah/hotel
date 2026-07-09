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

public class InsertReservationCommandHandler(
    IReservationRepository reservationRepository,
    IRoomRepository roomRepository,
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
        var rootError = new Error($"insert reservation for guest {request.GuestId} and room {request.RoomId} failed.");

        var currentUserInfoResult = await currentUserService.GetUserInfoAsync(ct);
        if (!currentUserInfoResult.Succeeded)
            return Result<ReservationDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            var roomExists = await roomRepository.ExistsAsync(request.RoomId, ct);
            if (!roomExists)
                return Result<ReservationDto>.Failure([rootError, new Error($"room not found.")]);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            var managesRoomResult = await managerService.ManagesRoomAsync(currentUserInfo.id, request.RoomId, ct);
            if (!managesRoomResult.Succeeded)
                return Result<ReservationDto>.Failure(managesRoomResult.Errors.Prepend(rootError));
            var managesRoom = managesRoomResult.Value;

            if (!managesRoom)
                return Result<ReservationDto>.Failure([rootError, new Error($"room not found.")]);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            
        }
        else
        {
            return Result<ReservationDto>.Failure([rootError, new Error("forbidden request.", ErrorCode.Forbidden)],
                ResultCode.Forbidden);
        }

        var reservation = mapper.Map<Reservation>(request);
        return await CreateReservation(reservation, rootError, ct);
    }

    private async Task<Result<ReservationDto>> CreateReservation(
        Reservation reservation,
        Error rootError,
        CancellationToken ct)
    {
        var isReserved = await reservationRepository.IsRoomReservedAsync(reservation.RoomId, reservation.CheckInDate,
            reservation.CheckOutDate,
            ct);
        if (isReserved)
            return Result<ReservationDto>.Failure([rootError, new Error($"room is reserved.")]);

        var totalPriceResult = await reservationService.CalculatePriceAsync(reservation, ct);
        if (!totalPriceResult.Succeeded)
            return Result<ReservationDto>.Failure(totalPriceResult.Errors.Prepend(rootError));
        var totalPrice = totalPriceResult.Value;

        reservation.TotalPrice = totalPrice;
        reservation.Status = ReservationStatus.Confirmed;

        var reservationCreateResult = await reservationRepository.InsertAsync(reservation, ct);
        return mapper.Map<Result<ReservationDto>>(reservationCreateResult);
    }
}