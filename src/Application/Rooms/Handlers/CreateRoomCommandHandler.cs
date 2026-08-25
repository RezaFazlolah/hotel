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

public class CreateRoomCommandHandler(
    IRoomRepository roomRepository,
    IHotelRepository hotelRepository,
    IManagerRepository managerRepository,
    ICurrentUserService currentUserService,
    IMapper mapper)
    : IRequestHandler<CreateRoomCommand, Result<RoomDto>>
{
    public async Task<Result<RoomDto>> Handle(
        CreateRoomCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"insert room failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<RoomDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            var hotelExists = await hotelRepository.ExistsAsync(request.HotelId, ct);
            if (!hotelExists)
                return Result<RoomDto>.Failure(
                    [rootError, new Error($"hotel {request.HotelId} not found", ErrorCode.NotFound)],
                    ResultCode.NotFound);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            var managesHotel = await managerRepository.ManagesHotel(currentUserInfo.id, request.HotelId, ct);
            if (!managesHotel)
                return Result<RoomDto>.Failure(
                    [rootError, new Error($"hotel {request.HotelId} not found", ErrorCode.NotFound)],
                    ResultCode.NotFound);
        }
        else
        {
            return Result<RoomDto>.Forbidden(rootError);
        }

        var roomNumberExists = await roomRepository.NumberExistsAsync(request.HotelId, request.Number, ct);
        if (roomNumberExists)
            return Result<RoomDto>.Failure([
                rootError, new Error($"hotel {request.HotelId} already has room number {request.Number}")
            ]);

        var room = mapper.Map<Room>(request);
        
        var result = await roomRepository.AddAsync(room, ct);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return Result<RoomDto>.Handle(resultDto, rootError);
    }
}