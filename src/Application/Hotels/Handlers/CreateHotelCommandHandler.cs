using Application.Common.Extensions;
using Application.Hotels.Commands;
using Application.Hotels.Dtos;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Hotels.Handlers;

public class CreateHotelCommandHandler(
    IHotelRepository hotelRepository,
    IManagerRepository managerRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<CreateHotelCommand, Result<HotelDto>>
{
    public async Task<Result<HotelDto>> Handle(
        CreateHotelCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"insert hotel failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<HotelDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (!currentUserInfo.roles.Contains(UserRole.Admin))
            return Result<HotelDto>.Forbidden(rootError);

        var hotel = mapper.Map<Hotel>(request);

        if (request.ManagerId.HasValue)
        {
            var managerId = request.ManagerId.Value;
            
            var managerResult = await managerRepository.GetByIdAsync(managerId, ct);
            if (!managerResult.Succeeded)
                return Result<HotelDto>.Failure(managerResult.Errors.Prepend(rootError));
            var manager = (Manager)managerResult.Value;

            var managerAssignmentResult = hotel.AssignManager(manager);
            if (!managerAssignmentResult.Succeeded)
                return Result<HotelDto>.Failure(managerAssignmentResult.Errors.Prepend(rootError));
        }

        var result = await hotelRepository.AddAsync(hotel, ct);
        await unitOfWork.SaveChangesAsync(ct);
        var resultDto = result.Map<Hotel, HotelDto>(mapper);
        return Result<HotelDto>.Handle(resultDto, rootError);
    }
}