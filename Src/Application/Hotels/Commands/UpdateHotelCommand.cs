using Application.Hotels.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Commands;

public record UpdateHotelCommand
    : IRequest<Result<HotelDto>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public float Rating { get; set; }
}