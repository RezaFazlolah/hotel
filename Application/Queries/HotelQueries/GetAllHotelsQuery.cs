using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Queries.HotelQueries;

public class GetAllHotelsQuery : IRequest<Result<ICollection<Hotel>>>
{
    // filtering
    public string? FilterOn { get; set; }
    public string? FilterQuery { get; set; }
    // sorting
    public string? OrderBy { get; set; }
    public bool IsAscending { get; set; } = true;
    // pagination
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = int.MaxValue;
}