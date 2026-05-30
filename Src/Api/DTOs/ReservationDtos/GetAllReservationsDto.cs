using SharedKernel.Paging;

namespace Api.DTOs.ReservationDtos;

public class GetAllReservationsDto
{
    public PaginationParameters PaginationParameters { get; set; } = new();
}