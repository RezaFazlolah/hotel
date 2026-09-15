using Application.Common.Extensions;
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

public class UpdateReservationAsGuestCommandHandler(
    ICurrentUserService currentUserService,
    IReservationRepository reservationRepository,
    IReservationService reservationService,
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

        var ownsReservation = await reservationRepository.IsReservedByGuestAsync(request.ReservationId, userInfo.id, ct);
        if (!ownsReservation)
            return Result<ReservationDto>.Failure([rootError, new Error($"reservation not found", ErrorCode.NotFound)],
                ResultCode.NotFound);

        var reservationResult = await reservationRepository.GetByIdAsync(request.ReservationId, ct);
        if (!reservationResult.Succeeded)
            return Result<ReservationDto>.Failure(reservationResult.Errors.Prepend(rootError));
        var reservation = reservationResult.Value;

        var isReserved = await reservationRepository.IsRoomReservedAsync(request.ReservationId, reservation.RoomId,
            request.CheckInDate, request.CheckOutDate, ct);
        if (isReserved)
            return Result<ReservationDto>.Failure([rootError, new Error("room is reserved")], ResultCode.Conflict);

        mapper.Map(request, reservation);
        var totalPriceResult = await reservationService.CalculatePriceAsync(reservation, ct);
        if (!totalPriceResult.Succeeded)
            return Result<ReservationDto>.Failure(totalPriceResult.Errors.Prepend(rootError));

        var updateResult = await reservationRepository.UpdateWithReloadAsync(reservation, ct);
        var updateResultDto = updateResult.Map<Reservation, ReservationDto>(mapper);
        return Result<ReservationDto>.Handle(updateResultDto, rootError);
    }
}