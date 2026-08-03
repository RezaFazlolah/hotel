using Application.Hotels.Dtos;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Commands;

public record InsertHotelCommand
    : IRequest<Result<HotelDto>>
{
    public required string Name { get; init; }
    public required string Address { get; init; }
    public float Rating { get; init; }
    public Guid ManagerId { get; init; }
}