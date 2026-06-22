using Application.Dtos.ReservationDtos;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Reservations.Commands;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Reservations.Handlers;

public class CancelReservationCommandHandler(
    IReservationRepository reservationRepository,
    IMapper mapper)
    : IRequestHandler<CancelReservationCommand, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
    {
        if (!await reservationRepository.ExistsAsync(request.ReservationId, cancellationToken))
            return Result<ReservationDto>.Failure(new Error($"reservation {request.ReservationId} not found"),
                ResultCode.NotFound);

        var result = await reservationRepository.CancelAsync(request.ReservationId, cancellationToken);
        return mapper.Map<Result<ReservationDto>>(result);
    }
}