using SharedKernel.Paging;

namespace Api.DTOs.RoomDtos;

public class GetAllRoomsDto
{
    public PaginationParameters PaginationParameters { get; set; }
}