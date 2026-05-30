using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Requests.RoomRequests;

public class GetAllRooms : IRequest<Result<PagedResult<Room>>>
{
    public PaginationParameters PaginationParameters { get; set; } = new();
    // public RoomFilterParameters FilterParameters { get; set; }
}