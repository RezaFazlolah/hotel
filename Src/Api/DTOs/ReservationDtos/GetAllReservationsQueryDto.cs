using SharedKernel.Paging;

namespace Api.DTOs.ReservationDtos;

public class GetAllReservationsQueryDto
{
    public PaginationParameters PaginationParameters { get; set; } = new();
}