namespace Api.Dtos.ReservationDtos;

// Future: use inheritance for GetAllHotelsQueryDto, GetAllRoomsQueryDto, GetAllReservationsQueryDto 
public class GetAllReservationsQueryDto
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}