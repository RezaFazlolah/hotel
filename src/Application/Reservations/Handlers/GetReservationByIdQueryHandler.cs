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

        Result<ReservationDto> result;

        // var reservationDtoResult = await reservationQueryService.GetByIdAsync(request.Id, ct);
        // if (!reservationDtoResult.Succeeded)
        // return Result<ReservationDto>.Failure(reservationDtoResult.Errors.Prepend(rootError));
        // var reservationDto = reservationDtoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            result = await reservationQueryService.GetByIdAsync(request.Id, ct);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            // return await managerRepository.ManagesRoomAsync(currentUserInfo.id, reservationDto.RoomId, ct)
            //     ? reservationDtoResult
            //     : Result<ReservationDto>.Failure([rootError, new Error("reservation not found", ErrorCode.NotFound)],
            //         ResultCode.NotFound);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Guest))
        {
            // return reservationDto.GuestId == currentUserInfo.id
            //     ? reservationDtoResult
            //     : Result<ReservationDto>.Failure([rootError, new Error("reservation not found")]);
        }
        else
        {
            return Result<ReservationDto>.Failure([rootError, new Error("forbidden request", ErrorCode.Forbidden)],
                ResultCode.Forbidden);
        }

        throw new NotImplementedException();
    }
}