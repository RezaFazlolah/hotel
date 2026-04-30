using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Queries.HotelQueries;

public class GetHotelByIdQuery : IRequest<Result<Hotel>>
{
    public Guid HotelId { get; set; }
}