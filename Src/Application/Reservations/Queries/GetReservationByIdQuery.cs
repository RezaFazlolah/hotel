using MediatR;
using SharedKernel.Common;

namespace Application.Reservations.Queries;

public record GetReservationByIdQuery(Guid UserId, Guid ReservationId)
    : IRequest<Result<Domain.Models.Reservation>>;