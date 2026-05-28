using SharedKernel.Filtering;

namespace Api.DTOs.HotelDtos;

public class GetAllHotelsDto
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public HotelFilterParameters FilterParameters { get; set; }
}