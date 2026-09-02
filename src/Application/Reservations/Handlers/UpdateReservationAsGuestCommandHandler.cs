using Application.Common.Extensions;
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

public class UpdateReservationAsGuestCommandHandler(
    ICurrentUserService currentUserService,
    IReservationRepository reservationRepository,
    IMapper mapper)
    : IRequestHandler<UpdateReservationAsGuestCommand, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(
        UpdateReservationAsGuestCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"update reservation {request.ReservationId} failed");
        var userInfoResult = currentUserService.Info;
        if (!userInfoResult.Succeeded)
            return Result<ReservationDto>.Failure(userInfoResult.Errors.Prepend(rootError));
        var userInfo = userInfoResult.Value;

        if (!userInfo.roles.Contains(UserRole.Guest))
            return Result<ReservationDto>.Forbidden(rootError);

        var reservationResult = await reservationRepository.GetByIdAsync(request.ReservationId, ct);
        if (!reservationResult.Succeeded)
            return Result<ReservationDto>.Failure(reservationResult.Errors.Prepend(rootError));
        var reservation = reservationResult.Value;

        if (reservation.GuestId != userInfo.id)
            return Result<ReservationDto>.Failure([rootError, new Error($"reservation not found", ErrorCode.NotFound)],
                ResultCode.NotFound);

        var isReserved = await reservationRepository.IsRoomReservedAsync(reservation.RoomId, userInfo.id, request.CheckInDate,
            request.CheckOutDate, ct);
        if (isReserved)
            return Result<ReservationDto>.Failure([rootError, new Error("room is reserved")]);

        mapper.Map(request, reservation);
        var updateResult = await reservationRepository.UpdateWithReloadAsync(reservation, ct);
        var updateResultDto = updateResult.Map<Reservation, ReservationDto>(mapper);
        return Result<ReservationDto>.Handle(updateResultDto, rootError);
    }
}