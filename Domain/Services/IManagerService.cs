using Domain.Models;

namespace Domain.Services;

public interface IManagerService : IUserService
{
    Hotel? GetHotelByManagerIdAsync(Guid managerId);
}