using Application.Hotels.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Commands;

public record UpdateHotelCommand
    : IRequest<Result<HotelDto>>
{
    public required Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Address { get; init; }
    public float? Rating { get; init; }
    public Guid? ManagerId { get; init; }
}