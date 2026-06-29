using Application.Hotels.Dtos;
using Domain.Models;

namespace Application.Interfaces.QueryServices;

public interface IHotelQueryService
    : IBaseQueryService<Hotel, HotelDto>
{
}