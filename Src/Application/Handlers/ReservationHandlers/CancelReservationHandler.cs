using Application.Interfaces.Repositories;
using Application.Requests.ReservationRequests;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.ReservationHandlers;

public class CancelReservationHandler(
    IReservationRepository reservationRepository,
    IMapper mapper,
    ICurrentUserService currentUserService)
    : IRequestHandler<CancelReservation, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(CancelReservation request, CancellationToken cancellationToken)
    {
        if (!await reservationRepository.ExistsAsync(request.ReservationId, cancellationToken))
            return Result<Reservation>.Failure(new Error($"reservation {request.ReservationId} not found"),
                ResultCode.NotFound);

        return await reservationRepository.CancelAsync(request.ReservationId, cancellationToken);
    }
}