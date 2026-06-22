using SharedKernel.Paging;

namespace Api.Dtos.HotelDtos;

public class GetAllHotelsQueryDto
{
    public PaginationParameters PaginationParameters { get; set; } = new();
    // public HotelFilterParameters FilterParameters { get; set; }
}