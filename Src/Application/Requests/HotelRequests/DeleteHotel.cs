using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Requests.HotelRequests;

public class DeleteHotel : IRequest<Result<Hotel>>
{
    public required Guid HotelId { get; set; }
}