using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Requests.HotelRequests;

public class GetAllHotels : IRequest<Result<PagedResult<Hotel>>>
{
    public required PaginationParameters PaginationParameters { get; set; }
    // public HotelFilterParameters FilterParameters { get; set; }
}