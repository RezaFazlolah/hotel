using Application.Rooms.Dtos;
using Domain.Models;

namespace Application.Interfaces.Services.Query;

public interface IRoomQueryService
    : IBaseQueryService<Room, RoomDto>
{
}