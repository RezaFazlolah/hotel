using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Reservations.Commands;
using Application.Reservations.Dtos;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Reservations.Handlers;

public class CancelReservationCommandHandler(
    ICurrentUserService currentUserService,
    IReservationRepository reservationRepository,
    IManagerService  managerService,
    IMapper mapper)
    : IRequestHandler<CancelReservationCommand, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(
        CancelReservationCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"cancel reservation {request.ReservationId} failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<ReservationDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        var reservationResult = await reservationRepository.GetByIdAsync(request.ReservationId, ct);
        if (!reservationResult.Succeeded)
            return Result<ReservationDto>.Failure(reservationResult.Errors.Prepend(rootError));
        var reservation = reservationResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            if(!await managerService.ManagesRoomAsync(currentUserInfo.id, reservation.RoomId, ct))
                return Result<ReservationDto>.Failure([rootError,  new Error("reservation not found")]);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            if (reservation.GuestId != currentUserInfo.id)
                return Result<ReservationDto>.Failure([rootError, new Error("reservation not found")]);
        }
        
        if (reservation.Status == ReservationStatus.Cancelled)
            return Result<ReservationDto>.Failure([rootError, new Error("reservation is already cancelled")]);

        reservation.Status = ReservationStatus.Cancelled;
        var reservationCancelResult = await reservationRepository.UpdateAsync(reservation, ct);

        return reservationCancelResult.Succeeded
            ? mapper.Map<Result<ReservationDto>>(reservationCancelResult.Value)
            : Result<ReservationDto>.Failure(reservationCancelResult.Errors.Prepend(rootError));
    }
}