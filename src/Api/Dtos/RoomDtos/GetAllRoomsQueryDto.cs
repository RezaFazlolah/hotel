namespace Api.Dtos.RoomDtos;

// Future: use inheritance for GetAllHotelsQueryDto, GetAllRoomsQueryDto, GetAllReservationsQueryDto 
public class GetAllRoomsQueryDto
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}