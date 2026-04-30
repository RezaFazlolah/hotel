using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Commands.HotelCommands;

public class InsertHotelCommand : IRequest<Result<Hotel>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public float Rating { get; set; }
}