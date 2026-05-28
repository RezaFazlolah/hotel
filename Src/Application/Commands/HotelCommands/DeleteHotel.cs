using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Commands.HotelCommands;

public class DeleteHotel : IRequest<Result<Hotel>>
{
    public required Guid HotelId { get; set; }
}