using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Queries.HotelQueries;

public class GetHotelByIdQuery : IRequest<Result<Hotel>>
{
    public required Guid HotelId { get; set; }
}