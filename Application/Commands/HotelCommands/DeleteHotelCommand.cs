using Application.Result;
using Domain.Models;
using MediatR;

namespace Application.Commands.HotelCommands;

public class DeleteHotelCommand : IRequest<Result<Hotel>>
{
    public Guid HotelId { get; set; }
}