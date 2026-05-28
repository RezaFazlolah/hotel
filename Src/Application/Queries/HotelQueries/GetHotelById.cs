using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Queries.HotelQueries;

public class GetHotelById : IRequest<Result<Hotel>>
{
    public required Guid HotelId { get; set; }
}