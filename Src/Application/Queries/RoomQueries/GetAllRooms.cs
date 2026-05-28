using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Filtering;
using SharedKernel.Paging;

namespace Application.Queries.RoomQueries;

public class GetAllRooms : IRequest<Result<PagedResult<Room>>>
{
    public PaginationParameters PaginationParameters { get; set; }
    // public RoomFilterParameters FilterParameters { get; set; }
}