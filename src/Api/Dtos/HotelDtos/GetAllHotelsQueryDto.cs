namespace Api.Dtos.HotelDtos;

// Future: use inheritance for GetAllHotelsQueryDto, GetAllRoomsQueryDto, GetAllReservationsQueryDto 
public class GetAllHotelsQueryDto
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}