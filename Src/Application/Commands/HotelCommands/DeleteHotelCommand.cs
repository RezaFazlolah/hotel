using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Commands.HotelCommands;

public class DeleteHotelCommand : IRequest<Result<Hotel>>
{
    public required Guid HotelId { get; set; }
}