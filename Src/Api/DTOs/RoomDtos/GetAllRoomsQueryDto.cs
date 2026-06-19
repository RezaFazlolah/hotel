using SharedKernel.Paging;

namespace Api.DTOs.RoomDtos;

public class GetAllRoomsQueryDto
{
    public PaginationParameters PaginationParameters { get; set; } = new();
}