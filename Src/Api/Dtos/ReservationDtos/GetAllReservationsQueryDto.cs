using SharedKernel.Paging;

namespace Api.Dtos.ReservationDtos;

public class GetAllReservationsQueryDto
{
    public PaginationParameters PaginationParameters { get; set; } = new();
}