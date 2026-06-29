using Application.Hotels.Dtos;
using Domain.Models;

namespace Application.Interfaces.Services.Query;

public interface IHotelQueryService
    : IBaseQueryService<Hotel, HotelDto>
{
}