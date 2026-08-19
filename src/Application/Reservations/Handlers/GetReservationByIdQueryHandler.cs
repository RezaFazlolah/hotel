using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Reservations.Dtos;
using Application.Reservations.Queries;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Reservations.Handlers;

public class GetReservationByIdQueryHandler(
    IReservationQueryService reservationQueryService,
    ICurrentUserService currentUserService,
    IManagerRepository managerRepository)
    : IRequestHandler<GetReservationByIdQuery, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(
        GetReservationByIdQuery request,
        CancellationToken ct)
    {
        var rootError = new Error($"get reservation {request.Id} failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<ReservationDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (!currentUserService.IsAuthenticated())
            return Result<ReservationDto>.Forbidden(rootError);

        var reservationResult = await reservationQueryService.GetByIdAsync(request.Id, ct);
        if (!reservationResult.Succeeded)
            return Result<ReservationDto>.Failure(reservationResult.Errors.Prepend(rootError));
        var reservation = reservationResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            if (!await managerRepository.ManagesRoomAsync(currentUserInfo.id, reservation.RoomId, ct))
            {
                return Result<ReservationDto>.Failure([rootError, new Error("reservation not found", ErrorCode.NotFound)],
                    ResultCode.NotFound);
            }
        }
        else // if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            if (reservation.GuestId != currentUserInfo.id)
                return Result<ReservationDto>.Failure([rootError, new Error("reservation not found", ErrorCode.NotFound)],
                    ResultCode.NotFound);
        }

        return reservationResult;
    }
}