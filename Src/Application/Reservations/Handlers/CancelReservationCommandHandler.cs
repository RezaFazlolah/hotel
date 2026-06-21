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
    IMapper mapper,
    ICurrentUserService currentUserService)
    : IRequestHandler<CancelReservationCommand, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
    {
        if (!await reservationRepository.ExistsAsync(request.ReservationId, cancellationToken))
            return Result<Reservation>.Failure(new Error($"reservation {request.ReservationId} not found"),
                ResultCode.NotFound);

        return await reservationRepository.CancelAsync(request.ReservationId, cancellationToken);
    }
}