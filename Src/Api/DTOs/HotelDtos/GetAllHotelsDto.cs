using SharedKernel.Filtering;
using SharedKernel.Paging;

namespace Api.DTOs.HotelDtos;

public class GetAllHotelsDto
{
    public PaginationParameters PaginationParameters { get; set; } = new();
    // public HotelFilterParameters FilterParameters { get; set; }
}