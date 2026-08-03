using Application.Reservations.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Reservations.Queries;

public record GetReservationByIdQuery(Guid Id)
    : IRequest<Result<ReservationDto>>;