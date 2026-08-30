using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Reservations.Commands;
using Application.Reservations.Dtos;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Reservations.Handlers;

public class UpdateReservationCommandHandler(
    IReservationRepository reservationRepository,
    ICurrentUserService currentUserService,
    IManagerRepository managerRepository,
    IMapper mapper)
    : IRequestHandler<UpdateReservationCommand, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(
        UpdateReservationCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"update reservation {request.Id} failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<ReservationDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        var reservationResult = await reservationRepository.GetByIdAsync(request.Id, ct);
        if (!reservationResult.Succeeded)
            return Result<ReservationDto>.Failure(reservationResult.Errors.Prepend(rootError));
        var reservation = reservationResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            if (!await managerRepository.ManagesRoomAsync(currentUserInfo.id, reservation.RoomId, ct))
                return Result<ReservationDto>.Failure([rootError, new Error($"reservation not found")]);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            if (reservation.GuestId != currentUserInfo.id)
                return Result<ReservationDto>.Failure([rootError, new Error($"reservation not found")]);
        }
        else
        {
            return Result<ReservationDto>.Failure([rootError, new Error($"forbidden request", ErrorCode.Forbidden)]);
        }

        if (reservation.Status == ReservationStatus.Cancelled)
            return Result<ReservationDto>.Failure([rootError, new Error("reservation is cancelled")]);

        var updatedReservation = new Reservation
        {
            GuestId = reservation.GuestId,
            RoomId = reservation.RoomId,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate
        };

        var isRoomReserved = await reservationRepository.IsRoomReservedAsync(updatedReservation.RoomId,
            updatedReservation.GuestId,
            updatedReservation.CheckInDate, updatedReservation.CheckOutDate, ct);
        if (isRoomReserved)
            return Result<ReservationDto>.Failure([rootError, new Error("another room is already reserved")]);

        var reservationUpdateResult = await reservationRepository.UpdateAsync(updatedReservation, ct);
        return reservationUpdateResult.Succeeded
            ? mapper.Map<Result<ReservationDto>>(reservationUpdateResult)
            : Result<ReservationDto>.Failure(reservationUpdateResult.Errors.Prepend(rootError));
    }
}