using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Rooms.Queries;

public class GetAllRoomsQuery
    : IRequest<Result<PagedResult<Domain.Models.Room>>>
{
    public PaginationParameters PaginationParameters { get; set; } = new();
    // public RoomFilterParameters FilterParameters { get; set; }
}