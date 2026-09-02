using System.Text.Json.Serialization;
using Application.Reservations.Dtos;
using MediatR;

namespace Api.Dtos.ReservationDtos;

public record UpdateReservationBaseCommandDto
    :IRequest<ReservationDto>
{
    public required DateTimeOffset CheckInDate { get; init; }
    public required DateTimeOffset CheckOutDate { get; init; }
}