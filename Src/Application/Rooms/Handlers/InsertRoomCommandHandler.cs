using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Rooms.Commands;
using Application.Rooms.Dtos;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Rooms.Handlers;

public class InsertRoomCommandHandler(
    IRoomRepository roomRepository,
    IHotelRepository hotelRepository,
    ICurrentUserService currentUserService,
    IManagerRepository managerRepository,
    IMapper mapper)
    : IRequestHandler<InsertRoomCommand, Result<RoomDto>>
{
    public async Task<Result<RoomDto>> Handle(InsertRoomCommand request, CancellationToken ct)
    {
        var rootError = new Error($"insert room failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<RoomDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            if (!await hotelRepository.ExistsAsync(request.HotelId, ct))
                return Result<RoomDto>.Failure([rootError, new Error($"hotel {request.HotelId} not found")]);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            var hotelIdResult = await managerRepository.GetHotelIdAsync(currentUserInfo.id, ct);
            if (!hotelIdResult.Succeeded)
                return Result<RoomDto>.Failure(hotelIdResult.Errors.Prepend(rootError));
            var hotelId = hotelIdResult.Value;

            if (request.HotelId != hotelId)
                return Result<RoomDto>.Failure([rootError, new Error($"hotel {request.HotelId} not found")]);
        }
        else
            return Result<RoomDto>.Forbidden(rootError);

        var roomNumberExistsResult =
            await roomRepository.RoomNumberExistsAsync(request.HotelId, request.Number, ct);
        if (!roomNumberExistsResult.Succeeded)
            return Result<RoomDto>.Failure(roomNumberExistsResult.Errors.Prepend(rootError));
        var roomNumberExists = roomNumberExistsResult.Value;

        if (roomNumberExists)
            return Result<RoomDto>.Failure([
                rootError, new Error($"hotel {request.HotelId} already has room number {request.Number}")
            ]);

        var result = await roomRepository.InsertAsync(mapper.Map<Room>(request), ct);
        return result.Succeeded
            ? mapper.Map<Result<RoomDto>>(result)
            : Result<RoomDto>.Failure(result.Errors.Prepend(rootError));
    }
}