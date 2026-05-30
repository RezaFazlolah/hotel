using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Requests.HotelRequests;

public class UpdateHotel : IRequest<Result<Hotel>>
{
    public required Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public float Rating { get; set; }
}