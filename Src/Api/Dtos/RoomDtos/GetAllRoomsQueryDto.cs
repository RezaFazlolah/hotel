using SharedKernel.Paging;

namespace Api.Dtos.RoomDtos;

public class GetAllRoomsQueryDto
{
    public PaginationParameters PaginationParameters { get; set; } = new();
}