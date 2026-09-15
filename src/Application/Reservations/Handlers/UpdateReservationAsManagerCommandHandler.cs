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

public class UpdateReservationAsManagerCommandHandler(
    ICurrentUserService currentUserService,
    IReservationRepository reservationRepository,
    IRoomRepository roomRepository,
    IReservationService reservationService,
    IMapper mapper)
    : IRequestHandler<UpdateReservationAsManagerCommand, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(
        UpdateReservationAsManagerCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"update reservation {request.ReservationId} failed");

        var userInfoResult = currentUserService.Info;
        if (!userInfoResult.Succeeded)
            return Result<ReservationDto>.Failure(userInfoResult.Errors.Prepend(rootError));
        var userInfo = userInfoResult.Value;

        if (!userInfo.roles.Contains(UserRole.Manager))
        {
            var result = Result<ReservationDto>.Forbidden(rootError);
            return result;
        }

        var managesReservation = await reservationRepository.IsManagedByManagerAsync(request.ReservationId, userInfo.id, ct);
        if (!managesReservation)
            return Result<ReservationDto>.Failure([rootError, new Error($"reservation not found", ErrorCode.NotFound)],
                ResultCode.NotFound);

        var reservationResult = await reservationRepository.GetByIdAsync(request.ReservationId, ct);
        if (!reservationResult.Succeeded)
            return Result<ReservationDto>.Failure(reservationResult.Errors.Prepend(rootError));
        var reservation = reservationResult.Value;

        var managesRoom = await roomRepository.IsManagedByManagerAsync(request.RoomId, userInfo.id, ct);
        if (!managesRoom)
            return Result<ReservationDto>.Failure([rootError, new Error($"room {request.RoomId} not found", ErrorCode.NotFound)],
                ResultCode
                    .NotFound);

        var roomExists = await roomRepository.ExistsAsync(request.RoomId, ct);
        if (!roomExists)
            return Result<ReservationDto>.Failure([rootError, new Error($"room {request.RoomId} not found")],
                ResultCode.NotFound);

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